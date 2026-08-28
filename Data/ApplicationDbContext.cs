using Microsoft.EntityFrameworkCore;
using Gift_of_the_Givers_Relief_App.Models;

namespace Gift_of_the_Givers_Relief_App.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Disaster> Disasters { get; set; }
        public DbSet<ReliefProject> ReliefProjects { get; set; }
        public DbSet<VolunteerAssignment> VolunteerAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User -> Volunteer (1-to-1)
            modelBuilder.Entity<Volunteer>()
                .HasOne(v => v.User)
                .WithOne(u => u.Volunteer)
                .HasForeignKey<Volunteer>(v => v.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Donor (1-to-1)
            modelBuilder.Entity<Donor>()
                .HasOne(d => d.User)
                .WithOne(u => u.Donor)
                .HasForeignKey<Donor>(d => d.UserID)               .OnDelete(DeleteBehavior.Cascade);

            // Donor -> Donations (1-to-many)
            modelBuilder.Entity<Donation>()
                .HasOne(d => d.Donor)
                .WithMany(d => d.Donations)
                .HasForeignKey(d => d.DonorID)
                .OnDelete(DeleteBehavior.Cascade);

            // Disaster -> ReliefProjects (1-to-many)
            modelBuilder.Entity<ReliefProject>()
                .HasOne(rp => rp.Disaster)
                .WithMany(d => d.ReliefProjects)
                .HasForeignKey(rp => rp.DisasterID)
                .OnDelete(DeleteBehavior.Cascade);

            // Volunteer -> VolunteerAssignments (1-to-many)
            modelBuilder.Entity<VolunteerAssignment>()
                .HasOne(va => va.Volunteer)
                .WithMany(v => v.VolunteerAssignments)
                .HasForeignKey(va => va.VolunteerID)
                .OnDelete(DeleteBehavior.Cascade);

            // ReliefProject -> VolunteerAssignments (1-to-many)
            modelBuilder.Entity<VolunteerAssignment>()
                .HasOne(va => va.ReliefProject)
                .WithMany(rp => rp.VolunteerAssignments)
                .HasForeignKey(va => va.ReliefProjectID)
                .OnDelete(DeleteBehavior.Cascade);

            // Donation amount precision
            modelBuilder.Entity<Donation>()
                .Property(d => d.Amount)
                .HasPrecision(18, 2);
        }
    }
}