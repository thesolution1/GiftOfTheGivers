using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models;

public class ContactRequest
{
    public int ContactRequestId { get; set; }

    [Required, StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(256)]
    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsContacted { get; set; }
}