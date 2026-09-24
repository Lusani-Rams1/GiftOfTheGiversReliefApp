using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gift_of_the_Givers_Relief_App.Pages
{
    public class ContactUsModel : PageModel
    {
        public string Email { get; set; } = "info@giftofthegivers.org";
        public string Phone { get; set; } = "+27 21 555 0123";
        public string Address { get; set; } = "123 Relief Avenue, Cape Town, South Africa";

        public void OnGet()
        {
        }
    }
}