using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Gift_of_the_Givers_Relief_App.Models
{
    public class ReliefProject
    {
        [Key]
        public int ReliefProjectID { get; set; }

        [Required]
        public int DisasterID { get; set; }

        [Required]
        public string ProjectName { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public string Status { get; set; } = null!;

        public Disaster? Disaster { get; set; }

        public ICollection<VolunteerAssignment> VolunteerAssignments { get; set; } = new HashSet<VolunteerAssignment>();
    }
}