using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gift_of_the_Givers_Relief_App.Models
{
    public class Disaster
    {
        [Key]
        public int DisasterID { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;

        [Required]
        public string Location { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public string Status { get; set; } = null!;

        public ICollection<ReliefProject> ReliefProjects { get; set; } = new HashSet<ReliefProject>();
    }
}