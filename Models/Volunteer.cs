using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerID { get; set; }

        [Required]
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }

        // NEW - Link to Project
        [Required]
        [Display(Name = "Project")]
        public int ReliefProjectID { get; set; }
        [ForeignKey("ReliefProjectID")]
        public ReliefProject? ReliefProject { get; set; }

        [Required]
        public string Skills { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Availability")]
        public string AvailabilityStatus { get; set; } = "Standby";
    }
}