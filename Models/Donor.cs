using System.ComponentModel.DataAnnotations;

namespace Gift_of_the_Givers_Relief_App.Models
{
    public class Donor
    {
        [Key]
        public int DonorID { get; set; }

        public int UserID { get; set; }

        [MaxLength(255)]
        public string OrganizationName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Address { get; set; } = string.Empty;

        public User? User { get; set; }

        public ICollection<Donation> Donations { get; set; }
            = new List<Donation>();
    }
}