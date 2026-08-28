using System.ComponentModel.DataAnnotations;

namespace Gift_of_the_Givers_Relief_App.Models
{
    public class Disaster
    {
        [Key]
        public int DisasterID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Type { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Location { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        public ICollection<ReliefProject> ReliefProjects { get; set; }
            = new List<ReliefProject>();
    }
}