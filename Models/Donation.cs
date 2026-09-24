using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gift_of_the_Givers_Relief_App.Models
{
    public class Donation
    {
        [Key]
        public int DonationID { get; set; }

        [Required]
        public int DonorID { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public string DonationType { get; set; } = null!;

        [Required]
        public DateTime DonationDate { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        public Donor? Donor { get; set; }
    }
}