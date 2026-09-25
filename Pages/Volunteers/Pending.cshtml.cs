using Gift_of_the_Givers_Relief_App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Gift_of_the_Givers_Relief_App.Pages.Volunteers
{
    public class PendingModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public PendingModel(ApplicationDbContext context) => _context = context;

        public string Status { get; private set; } = "Pending";

        public async Task<IActionResult> OnGetAsync()
        {
            if (!Request.Cookies.TryGetValue("GgUserId", out var idStr)
                || !int.TryParse(idStr, out var userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var v = await _context.Volunteers.FirstOrDefaultAsync(x => x.UserID == userId);
            if (v is not null) Status = v.Status;
            return Page();
        }
    }
}