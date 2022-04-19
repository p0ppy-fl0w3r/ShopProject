using System.ComponentModel.DataAnnotations;

namespace MyShop.Models
{
    public class NewUserModel
    {

        [Required(ErrorMessage = "Please add a first name.")]
        [Display(Name = "First name")]
        [MaxLength(30, ErrorMessage = "First name should be 30 characters of less")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please add a last name.")]
        [Display(Name = "Last name")]
        [MaxLength(30, ErrorMessage = "Last name should be 30 characters of less")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please select your gender.")]
        public string Gender { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
