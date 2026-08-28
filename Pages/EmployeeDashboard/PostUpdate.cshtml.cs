using Gift_of_the_Givers_Relief_App.Filters;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers_Relief_App.Pages.EmployeeDashboard
{
    // Protected placeholder page letting employees create updates (demo).
    [EmployeeAuthorize]
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

            // Demo behavior: show a success message only.
            StatusMessage = "Update published (demo).";
            Input = new UpdateInput();
            return Page();
        }
    }
}