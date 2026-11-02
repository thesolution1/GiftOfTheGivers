using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class Donation : IValidatableObject
    {
        public int DonationID { get; set; }

        [Required]
        [Range(1, 100000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = "ZAR";

        [Required]
        public string DonationType { get; set; } = "One-Time";

        [Display(Name = "Recurring duration")]
        public int? RecurringMonths { get; set; }

        [Display(Name = "Recurring day of month")]
        public int? RecurringDay { get; set; }

        public DateTime DonationDate { get; set; } = DateTime.Now;

        public bool IsAnonymous { get; set; }

        public string? UserID { get; set; }

        public string? ReferenceNumber { get; set; }

        [MaxLength(50)]
        public string? TaxCertificateNumber { get; set; }

        public DateTime? TaxCertificateIssuedAt { get; set; }

        public string? TaxCertificateDonorName { get; set; }

        public string? TaxCertificateDonorEmail { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (DonationType == "Recurring")
            {
                if (!RecurringMonths.HasValue)
                {
                    yield return new ValidationResult(
                        "Select how many months the donation should recur.",
                        new[] { nameof(RecurringMonths) });
                }
                else if (RecurringMonths.Value < 3 ||
                         RecurringMonths.Value > 120)
                {
                    yield return new ValidationResult(
                        "Recurring donations must be between 3 and 120 months.",
                        new[] { nameof(RecurringMonths) });
                }

                if (!RecurringDay.HasValue)
                {
                    yield return new ValidationResult(
                        "Select the day of the month for the recurring donation.",
                        new[] { nameof(RecurringDay) });
                }
                else if (RecurringDay.Value < 1 ||
                         RecurringDay.Value > 31)
                {
                    yield return new ValidationResult(
                        "The recurring day must be between 1 and 31.",
                        new[] { nameof(RecurringDay) });
                }
            }
            else
            {
                RecurringMonths = null;
                RecurringDay = null;
            }
        }
    }
}