using Gift_of_the_Givers_Relief_App.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers_Relief_App.Pages.EmployeeDashboard
{
    // Require an employee to access this page.
    [EmployeeAuthorize]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}