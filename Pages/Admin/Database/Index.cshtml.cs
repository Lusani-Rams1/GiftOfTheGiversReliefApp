using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Gift_of_the_Givers_Relief_App.Data;

namespace Gift_of_the_Givers_Relief_App.Pages.Admin.Database
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public record TableInfo(string Name, int Count, string Link);

        public List<TableInfo> Tables { get; private set; } = new();

        public async Task OnGetAsync()
        {
            Tables = new List<TableInfo>
            {
                new TableInfo("Disasters", await _context.Disasters.CountAsync(), "/Admin/Database/Disasters"),
                new TableInfo("ReliefProjects", await _context.ReliefProjects.CountAsync(), "/Admin/Database/ReliefProjects"),
                new TableInfo("Users", await _context.Users.CountAsync(), "/Admin/Database/Users"),
                new TableInfo("Donors", await _context.Donors.CountAsync(), "/Admin/Database/Donors"),
                new TableInfo("Donations", await _context.Donations.CountAsync(), "/Admin/Database/Donations"),
                new TableInfo("Volunteers", await _context.Volunteers.CountAsync(), "/Admin/Database/Volunteers"),
                new TableInfo("VolunteerAssignments", await _context.VolunteerAssignments.CountAsync(), "/Admin/Database/VolunteerAssignments")
            };
        }
    }
}