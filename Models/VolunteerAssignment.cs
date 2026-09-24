using System;
using System.ComponentModel.DataAnnotations;

namespace Gift_of_the_Givers_Relief_App.Models
{
    public class VolunteerAssignment
    {
        [Key]
        public int AssignmentID { get; set; }

        [Required]
        public int VolunteerID { get; set; }

        [Required]
        public int ReliefProjectID { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; }

        [Required]
        public string Role { get; set; } = null!;

        [Required]
        public string Status { get; set; } = null!;

        public Volunteer? Volunteer { get; set; }
        public ReliefProject? ReliefProject { get; set; }
    }
}