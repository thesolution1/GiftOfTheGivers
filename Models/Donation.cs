using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class Donation
    {
        [Key]
        public int DonationID { get; set; }

        [Required]
        [Range(1, 100000000)]
        [Column(TypeName = "decimal(10,2)")] // Changed from 18,2 to 10,2 to match your SQL
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = "ZAR";

        [Required]
        public string DonationType { get; set; } = "One-Time";

        public DateTime DonationDate { get; set; } = DateTime.Now;

        public bool IsAnonymous { get; set; }

        // OLD: public string? UserID - NEW: int to match your Users table
        [Required]
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }

        // Keep your old field
        public string? ReferenceNumber { get; set; }
    }
}