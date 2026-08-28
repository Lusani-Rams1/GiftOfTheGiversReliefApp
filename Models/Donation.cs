using System.ComponentModel.DataAnnotations;

namespace Gift_of_the_Givers_Relief_App.Models
{
    public class Donation
    {
        [Key]
        public int DonationID { get; set; }

        public int DonorID { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [MaxLength(100)]
        public string DonationType { get; set; } = string.Empty;

        public DateTime DonationDate { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public Donor? Donor { get; set; }
    }
}