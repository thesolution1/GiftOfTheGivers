using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class ReliefProject
    {
        [Key]
        public int ProjectID { get; set; } // OLD was ReliefProjectID - SQL uses ProjectID

        [Required, Display(Name = "Project Name")]
        public string ProjectName { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        // Your old "Update" field is now Description
        [Display(Name = "Project Update / Description")]
        public string? Description { get; set; }

        [Display(Name = "Update Date")]
        public DateTime? StartDate { get; set; } // We use StartDate for your old UpdateDate

        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "Active";

        public ICollection<Volunteer> Volunteers { get; set; } = new List<Volunteer>();

        // NEW - who created it
        public int CreatedByUserID { get; set; }
        [ForeignKey("CreatedByUserID")]
        public User? CreatedByUser { get; set; }
    }
}