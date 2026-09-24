using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers_Relief_App.Pages.Donators
{
    public class TaxCertificateModel : PageModel
    {
        public class CertificateRequest
        {
            [Required]
            public string Reference { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;
        }

        [BindProperty]
        public CertificateRequest Request { get; set; } = new();

        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // demo behaviour: show a friendly message, do not persist
            Message = $"Request received for reference '{Request.Reference}'. A certificate would be emailed to {Request.Email} (demo).";

            // reset the form values for clarity in demo
            Request = new CertificateRequest();

            return Page();
        }
    }
}