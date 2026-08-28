using System.ComponentModel.DataAnnotations;

namespace Gift_of_the_Givers_Relief_App.Models
{
    public class VolunteerAssignment
    {
        [Key]
        public int AssignmentID { get; set; }

        public int VolunteerID { get; set; }

        public int ReliefProjectID { get; set; }

        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string Role { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        public Volunteer? Volunteer { get; set; }

        public ReliefProject? ReliefProject { get; set; }
    }
}