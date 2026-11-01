using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models;

public class UpcomingProjectRequest
{
    public int UpcomingProjectRequestId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Skills { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Availability { get; set; } = string.Empty;

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    public bool IsProcessed { get; set; }
}