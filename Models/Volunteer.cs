using System.ComponentModel.DataAnnotations;

namespace Gift_of_the_Givers_Relief_App.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerID { get; set; }

        public int UserID { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Skills { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Availability { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        public User? User { get; set; }

        public ICollection<VolunteerAssignment> VolunteerAssignments { get; set; }
            = new List<VolunteerAssignment>();
    }
}