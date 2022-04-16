using MyShop.Models;
using System.ComponentModel.DataAnnotations;

namespace MyShop.DTOs
{
    public class CopyDto
    {
        public Dvdcopy Dvdcopy{ get; set; }

        [Required(ErrorMessage = "Member id is required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Member id is invalid!")]
        public int MemberId { get; set; }
        public int CopyId { get; set; } 

        [Required(ErrorMessage ="The loan type is required.")]
        public int TypeId { get; set; }
    }
}
