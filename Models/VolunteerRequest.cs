using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models;

public class VolunteerRequest
{
    public int VolunteerRequestId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public int ReliefProjectId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Skills { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Availability { get; set; } = string.Empty;

    public VolunteerRequestStatus Status { get; set; } =
        VolunteerRequestStatus.Pending;

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public ReliefProject? ReliefProject { get; set; }
}