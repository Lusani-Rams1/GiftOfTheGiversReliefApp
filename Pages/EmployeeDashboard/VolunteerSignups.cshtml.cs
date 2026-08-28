using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers_Relief_App.Pages.EmployeeDashboard
{
    public class VolunteerSignupsModel : PageModel
    {
        public class DemoSignup
        {
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public string Availability { get; set; } = string.Empty;
        }

        public List<DemoSignup> SampleSignups { get; set; } = new();

        public void OnGet()
        {
            SampleSignups = new List<DemoSignup>
            {
                new DemoSignup { Name = "Sanelisiwe M.", Email = "sanele@example.com", Location = "Durban", Availability = "Weekends" },
                new DemoSignup { Name = "Thabo K.", Email = "thabo@example.com", Location = "Grahamstown", Availability = "Weekdays" },
                new DemoSignup { Name = "Aisha P.", Email = "aisha@example.com", Location = "Cape Town", Availability = "Flexible" }
            };
        }
    }
}