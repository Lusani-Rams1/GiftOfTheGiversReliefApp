using Gift_of_the_Givers_Relief_App.Data;
using Gift_of_the_Givers_Relief_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Gift_of_the_Givers_Relief_App.Pages.Admin.Volunteers
{
    public class VerifyModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public VerifyModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Volunteer> PendingVolunteers { get; private set; } = new();
        public List<Volunteer> ApprovedVolunteers { get; private set; } = new();
        public List<Volunteer> RejectedVolunteers { get; private set; } = new();

        public string? StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            var all = await _context.Volunteers
                .Include(v => v.User)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();

            PendingVolunteers = all.Where(v => v.Status == "Pending").ToList();
            ApprovedVolunteers = all.Where(v => v.Status == "Approved").ToList();
            RejectedVolunteers = all.Where(v => v.Status == "Rejected").ToList();
        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            var v = await _context.Volunteers.FindAsync(id);
            if (v is null) return NotFound();

            v.Status = "Approved";
            await _context.SaveChangesAsync();

            StatusMessage = $"Volunteer #{id} approved.";
            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            var v = await _context.Volunteers.FindAsync(id);
            if (v is null) return NotFound();

            v.Status = "Rejected";
            await _context.SaveChangesAsync();

            StatusMessage = $"Volunteer #{id} rejected.";
            await OnGetAsync();
            return Page();
        }
    }
}