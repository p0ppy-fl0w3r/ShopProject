using MyShop.Models;
using System.ComponentModel.DataAnnotations;

namespace MyShop.DTOs
{
    public class MemberDto
    {
        public MembershipCategory MemberCategory { get; set; }

        [Required]
        public string Address { get; set; } = null!;
        [StringLength(50)]
        [Required]
        public string FirstName { get; set; } = null!;
        [StringLength(50)]
        [Required]
        public string LastName { get; set; } = null!;
        
        public DateTime DateOfBirth { get; set; }


        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Please choose at least one image.")]
        public IFormFile MemberImage { get; set; }
    }
}
