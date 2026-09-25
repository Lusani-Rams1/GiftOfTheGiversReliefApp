using System.ComponentModel.DataAnnotations;
using Gift_of_the_Givers_Relief_App.Data;
using Gift_of_the_Givers_Relief_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Gift_of_the_Givers_Relief_App.Pages.Volonteers
{
    public class Volonteers_formModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher<User> _passwordHasher;

        public Volonteers_formModel(ApplicationDbContext db)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<User>();
        }

        public class InputModel
        {
            [Required] public string FullName { get; set; } = string.Empty;
            [Required, EmailAddress] public string Email { get; set; } = string.Empty;
            [Required] public string PhoneNumber { get; set; } = string.Empty;
            [Required] public string Location { get; set; } = string.Empty;

            public int? DisasterId { get; set; }          // nullable to match DisasterID
            [Required] public string Availability { get; set; } = string.Empty;
            public string? Skills { get; set; }
            public bool HasOwnTransport { get; set; }
            public string? Message { get; set; }

            [Required, MinLength(6)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? StatusMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emailLower = Input.Email.Trim().ToLowerInvariant();

            var existingUser = await _db.Users
                .SingleOrDefaultAsync(u => u.Email.ToLower() == emailLower);

            if (existingUser is not null)
            {
                ModelState.AddModelError(string.Empty,
                    "An account with this email already exists. Please log in instead.");
                return Page();
            }

            var parts = Input.FullName.Trim().Split(' ', 2);
            var firstName = parts[0];
            var lastName = parts.Length > 1 ? parts[1] : string.Empty;

            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = Input.Email.Trim(),
                Role = "Volunteer",
                CreatedAt = DateTime.UtcNow
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, Input.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var volunteer = new Volunteer
            {
                UserID = user.UserID,
                PhoneNumber = Input.PhoneNumber.Trim(),
                Skills = Input.Skills ?? string.Empty,
                Availability = Input.Availability,
                Status = "Pending",

                // Only if you added them via Option B:
                Location = Input.Location.Trim(),
                DisasterID = Input.DisasterId,
                HasOwnTransport = Input.HasOwnTransport,
                Message = Input.Message,
                CreatedAt = DateTime.UtcNow
            };

            _db.Volunteers.Add(volunteer);
            await _db.SaveChangesAsync();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("GgUserId", user.UserID.ToString(), cookieOptions);

            return RedirectToPage("/Index");
        }
    }
}