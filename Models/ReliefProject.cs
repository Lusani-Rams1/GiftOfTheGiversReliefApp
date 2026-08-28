using System.ComponentModel.DataAnnotations;

namespace Gift_of_the_Givers_Relief_App.Models
{
    public class ReliefProject
    {
        [Key]
        public int ReliefProjectID { get; set; }

        public int DisasterID { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProjectName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        public Disaster? Disaster { get; set; }

        public ICollection<VolunteerAssignment> VolunteerAssignments { get; set; }
            = new List<VolunteerAssignment>();
    }
}