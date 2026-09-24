using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Gift_of_the_Givers_Relief_App.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }

        public ICollection<Donor> Donors { get; set; } = new HashSet<Donor>();
        public ICollection<Volunteer> Volunteers { get; set; } = new HashSet<Volunteer>();
    }
}