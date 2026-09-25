using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Gift_of_the_Givers_Relief_App.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string Skills { get; set; } = null!;

        [Required]
        public string Availability { get; set; } = null!;

        [Required]
        public string Status { get; set; } = null!;

        public User? User { get; set; }
        public string? Location { get; set; }
        public int? DisasterID { get; set; }
        public bool HasOwnTransport { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<VolunteerAssignment> VolunteerAssignments { get; set; } = new HashSet<VolunteerAssignment>();
    }
}