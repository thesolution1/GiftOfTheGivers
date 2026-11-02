using System.ComponentModel.DataAnnotations;

public class VolunteerAvailability
{
    [Key] public int AvailabilityID { get; set; }
    public int VolunteerID { get; set; }
    public DateTime AvailableDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
}