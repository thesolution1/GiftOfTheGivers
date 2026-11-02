using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "User"; // User or Employee
    }
}