using System.ComponentModel.DataAnnotations;
using Gift_of_the_Givers_Relief_App.Data;
using Gift_of_the_Givers_Relief_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Gift_of_the_Givers_Relief_App.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher<User> _passwordHasher;

        public RegisterModel(ApplicationDbContext db)
        {
            _db = db;
            _passwordHasher = new PasswordHasher<User>();
        }

        public class InputModel
        {
            [Required]
            [MaxLength(100)]
            public string FirstName { get; set; } = string.Empty;

            [Required]
            [MaxLength(100)]
            public string LastName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [MaxLength(255)]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [MinLength(6)]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [MaxLength(50)]
            public string? Role { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var emailLower = Input.Email.Trim().ToLowerInvariant();

            if (await _db.Users.AnyAsync(u => u.Email.ToLower() == emailLower))
            {
                ModelState.AddModelError(string.Empty, "An account with that email already exists.");
                return Page();
            }

            var user = new User
            {
                FirstName = Input.FirstName.Trim(),
                LastName = Input.LastName.Trim(),
                Email = emailLower,
                Role = Input.Role?.Trim() ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, Input.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // lightweight sign-in: set a secure cookie with the user id
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