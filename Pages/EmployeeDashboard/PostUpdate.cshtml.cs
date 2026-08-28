using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers_Relief_App.Pages.EmployeeDashboard
{
    public class PostUpdateModel : PageModel
    {
        public class UpdateInput
        {
            [Required]
            [MaxLength(150)]
            public string Title { get; set; } = string.Empty;

            [Required]
            [MaxLength(2000)]
            public string Message { get; set; } = string.Empty;
        }

        [BindProperty]
        public UpdateInput Input { get; set; } = new();

        public string StatusMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // demo behaviour: show a success message but do not persist
            StatusMessage = "Update published (demo).";

            // clear form
            Input = new UpdateInput();

            return Page();
        }
    }
}