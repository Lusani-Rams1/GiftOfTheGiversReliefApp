using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gift_of_the_Givers_Relief_App.Data;

namespace Gift_of_the_Givers_Relief_App.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int DisasterCount { get; private set; }
        public int ReliefProjectCount { get; private set; }
        public int UserCount { get; private set; }
        public int DonorCount { get; private set; }
        public int VolunteerCount { get; private set; }
        public int VolunteerAssignmentCount { get; private set; }
        public decimal TotalDonations { get; private set; }

        public async Task OnGetAsync()
        {
            DisasterCount = await _context.Disasters.CountAsync();
            ReliefProjectCount = await _context.ReliefProjects.CountAsync();
            UserCount = await _context.Users.CountAsync();
            DonorCount = await _context.Donors.CountAsync();
            VolunteerCount = await _context.Volunteers.CountAsync();
            VolunteerAssignmentCount = await _context.VolunteerAssignments.CountAsync();

            // Sum of donations - safe fallback to 0 when table empty.
            TotalDonations = await _context.Donations.AnyAsync()
                ? await _context.Donations.SumAsync(d => d.Amount)
                : 0m;
        }
    }
}