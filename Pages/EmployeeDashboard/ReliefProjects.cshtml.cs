using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers_Relief_App.Pages.EmployeeDashboard
{
    public class ReliefProjectsModel : PageModel
    {
        public class DemoProject
        {
            public string Title { get; set; } = string.Empty;
            public string Region { get; set; } = string.Empty;
            public string Summary { get; set; } = string.Empty;
            public DateTime Started { get; set; }
        }

        public List<DemoProject> SampleProjects { get; set; } = new();

        public void OnGet()
        {
            // simple static demo data
            SampleProjects = new List<DemoProject>
            {
                new DemoProject { Title = "Food & Shelter Relief", Region = "KwaZulu-Natal", Summary = "Distributing food parcels and temporary shelter to affected households.", Started = DateTime.UtcNow.AddDays(-14) },
                new DemoProject { Title = "Water & Sanitation", Region = "Eastern Cape", Summary = "Providing clean water via tankering and sanitation kits.", Started = DateTime.UtcNow.AddDays(-30) },
                new DemoProject { Title = "Wildfire Recovery", Region = "Western Cape", Summary = "Material support and community rebuilding coordination.", Started = DateTime.UtcNow.AddDays(-60) }
            };
        }
    }
}