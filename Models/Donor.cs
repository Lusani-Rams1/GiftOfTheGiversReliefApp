using System.ComponentModel.DataAnnotations;

namespace Gift_of_the_Givers_Relief_App.Models
{
    public class Donor
    {
        [Key]
        public int DonorID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public string OrganizationName { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        public User? User { get; set; }

        public ICollection<Donation> Donations { get; set; } = new HashSet<Donation>();
    }
}