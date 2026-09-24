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

        public DbSet<Disaster> Disasters { get; set; } = null!;
        public DbSet<ReliefProject> ReliefProjects { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Donor> Donors { get; set; } = null!;
        public DbSet<Donation> Donations { get; set; } = null!;
        public DbSet<Volunteer> Volunteers { get; set; } = null!;
        public DbSet<VolunteerAssignment> VolunteerAssignments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table names (explicit to match your schema)
            modelBuilder.Entity<Disaster>().ToTable("Disasters");
            modelBuilder.Entity<ReliefProject>().ToTable("ReliefProjects");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Donor>().ToTable("Donors");
            modelBuilder.Entity<Donation>().ToTable("Donations");
            modelBuilder.Entity<Volunteer>().ToTable("Volunteers");
            modelBuilder.Entity<VolunteerAssignment>().ToTable("VolunteerAssignments");

            // Keys are picked up by convention, but express foreign keys explicitly:
            modelBuilder.Entity<ReliefProject>()
                .HasOne(r => r.Disaster)
                .WithMany(d => d.ReliefProjects)
                .HasForeignKey(r => r.DisasterID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Donor>()
                .HasOne(d => d.User)
                .WithMany(u => u.Donors)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Volunteer>()
                .HasOne(v => v.User)
                .WithMany(u => u.Volunteers)
                .HasForeignKey(v => v.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Donation>()
                .HasOne(d => d.Donor)
                .WithMany(dn => dn.Donations)
                .HasForeignKey(d => d.DonorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VolunteerAssignment>()
                .HasOne(va => va.Volunteer)
                .WithMany(v => v.VolunteerAssignments)
                .HasForeignKey(va => va.VolunteerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VolunteerAssignment>()
                .HasOne(va => va.ReliefProject)
                .WithMany(rp => rp.VolunteerAssignments)
                .HasForeignKey(va => va.ReliefProjectID)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure decimal precision for donations
            modelBuilder.Entity<Donation>()
                .Property(d => d.Amount)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}