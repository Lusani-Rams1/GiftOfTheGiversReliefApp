using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using Gift_of_the_Givers_Relief_App.Data;
using Gift_of_the_Givers_Relief_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Gift_of_the_Givers_Relief_App.Pages.Donators
{
    public class Donators_formModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _config;

        public Donators_formModel(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
            _passwordHasher = new PasswordHasher<User>();
        }

        public class InputModel
        {
            [Required] public string FullName { get; set; } = string.Empty;
            [Required, EmailAddress] public string Email { get; set; } = string.Empty;
            [Required] public string PhoneNumber { get; set; } = string.Empty;

            // Donor model requires these — collect them from the form
            public string? OrganizationName { get; set; }
            [Required] public string Address { get; set; } = string.Empty;

            [Required, Range(10, double.MaxValue, ErrorMessage = "Minimum donation is R10.")]
            public decimal Amount { get; set; }

            public string? DisasterOrCause { get; set; }
            public bool IsAnonymous { get; set; }
            public string? Message { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? StatusMessage { get; set; }
        public string? TaxCertificateJson { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var emailLower = Input.Email.Trim().ToLowerInvariant();

            // 1. Find or create the User
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == emailLower);

            if (user is null)
            {
                var parts = Input.FullName.Trim().Split(' ', 2);
                user = new User
                {
                    FirstName = parts[0],
                    LastName = parts.Length > 1 ? parts[1] : string.Empty,
                    Email = Input.Email.Trim(),
                    Role = "Donor",
                    CreatedAt = DateTime.UtcNow
                };

                // No password set — they can register later to set one.
                user.PasswordHash = _passwordHasher.HashPassword(user, Guid.NewGuid().ToString());

                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }

            // 2. Find or create the Donor
            var donor = await _db.Donors
                .FirstOrDefaultAsync(d => d.UserID == user.UserID);

            if (donor is null)
            {
                donor = new Donor
                {
                    UserID = user.UserID,
                    OrganizationName = Input.OrganizationName ?? "(Individual)",
                    PhoneNumber = Input.PhoneNumber.Trim(),
                    Address = Input.Address.Trim()
                };
                _db.Donors.Add(donor);
                await _db.SaveChangesAsync();
            }

            // 3. Create the Donation
            var donation = new Donation
            {
                DonorID = donor.DonorID,
                Amount = Input.Amount,
                DonationType = string.IsNullOrWhiteSpace(Input.DisasterOrCause)
                    ? "General"
                    : "Disaster Relief",
                DonationDate = DateTime.UtcNow,
                Description = Input.IsAnonymous
                    ? $"Anonymous donation of R{Input.Amount:N2} — {Input.DisasterOrCause ?? "General Fund"}"
                    : $"Donation from {user.FirstName} {user.LastName} of R{Input.Amount:N2} — {Input.DisasterOrCause ?? "General Fund"}"
            };

            _db.Donations.Add(donation);
            await _db.SaveChangesAsync();

            // 4. Call the Azure Function
            try
            {
                using var http = new HttpClient();

                var payload = new
                {
                    DonorName = Input.IsAnonymous
                        ? "Anonymous"
                        : $"{user.FirstName} {user.LastName}".Trim(),
                    DonorEmail = user.Email,
                    Amount = donation.Amount,
                    DonationDate = donation.DonationDate
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = _config["Functions:TaxCertificateUrl"];

                if (string.IsNullOrWhiteSpace(url))
                {
                    StatusMessage = "Certificate service is not configured.";
                    return Page();
                }

                var resp = await http.PostAsync(url, content);

                if (resp.IsSuccessStatusCode)
                    TaxCertificateJson = await resp.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                TaxCertificateJson = null;
            }

            StatusMessage = $"Thank you for your donation of R{donation.Amount:N2}! " +
                 $"Your donation reference is #{donation.DonationID}. " +
                 $"Save this number to request your tax certificate.";
            return Page();
        }
    }
}