using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        [MaxLength(30, ErrorMessage = "First name must be 30 characters or less.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "First name must be 30 characters or less.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }
    }
}
