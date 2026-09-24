using System.ComponentModel.DataAnnotations;
using Gift_of_the_Givers_Relief_App.Data;
using Gift_of_the_Givers_Relief_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Gift_of_the_Givers_Relief_App.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher<User> _passwordHasher;

        // demo employee credentials (displayed on the login page)
        private const string DemoEmployeeEmail = "employee@demo.local";
        private const string DemoEmployeePassword = "DemoPass123!";

        public LoginModel(ApplicationDbContext db)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<User>();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emailLower = Input.Email.Trim().ToLowerInvariant();

            // Special case: demo employee login — create account on demand
            if (emailLower == DemoEmployeeEmail && Input.Password == DemoEmployeePassword)
            {
                var employeeUser = await _db.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == emailLower);
                if (employeeUser is null)
                {
                    employeeUser = new User
                    {
                        FirstName = "Demo",
                        LastName = "Employee",
                        Email = DemoEmployeeEmail,
                        Role = "Employee",
                        CreatedAt = DateTime.UtcNow
                    };

                    employeeUser.PasswordHash = _passwordHasher.HashPassword(employeeUser, DemoEmployeePassword);

                    _db.Users.Add(employeeUser);
                    await _db.SaveChangesAsync();
                }

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                };

                Response.Cookies.Append("GgUserId", employeeUser.UserID.ToString(), cookieOptions);

                return RedirectToPage("/EmployeeDashboard/Index");
            }

            // regular user flow
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email.ToLower() == emailLower);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, Input.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }

            var cookieOpts = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("GgUserId", user.UserID.ToString(), cookieOpts);

            // if the user is an employee, send them to the Employee Dashboard
            if (string.Equals(user.Role?.Trim(), "Employee", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToPage("/EmployeeDashboard/Index");
            }

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToPage("/Index");
        }
    }
}