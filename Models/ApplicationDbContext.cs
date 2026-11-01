using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Models;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Donation> Donations { get; set; } = null!;

    public DbSet<Volunteer> Volunteers { get; set; } = null!;

    public DbSet<ReliefProject> ReliefProjects { get; set; } = null!;

    public DbSet<ContactRequest> ContactRequests { get; set; } = null!;

    public DbSet<VolunteerRequest> VolunteerRequests { get; set; } = null!;

    public DbSet<UpcomingProjectRequest> UpcomingProjectRequests { get; set; } =
        null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<VolunteerRequest>()
            .HasOne(request => request.ReliefProject)
            .WithMany()
            .HasForeignKey(request => request.ReliefProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<VolunteerRequest>()
            .HasIndex(request => new
            {
                request.UserId,
                request.ReliefProjectId
            })
            .IsUnique();
    }
}