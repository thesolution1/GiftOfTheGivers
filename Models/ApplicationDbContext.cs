using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Models
{
    public class ApplicationDbContext : DbContext // NOT IdentityDbContext anymore
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<ReliefProject> ReliefProjects { get; set; }
        public DbSet<VolunteerApplication> VolunteerApplications { get; set; }
        public DbSet<VolunteerAvailability> VolunteerAvailabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Your table names in SQL are exactly these, so we map them
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Donation>().ToTable("Donations");
        }
    }
}