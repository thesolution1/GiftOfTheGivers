using System.ComponentModel.DataAnnotations;

public class VolunteerApplication
{
    [Key] public int ApplicationID { get; set; }
    public int VolunteerID { get; set; }
    public int? ProjectID { get; set; }
    public DateTime ApplicationDate { get; set; } = DateTime.Now;
    public string Status { get; set; } = "Pending";
}

