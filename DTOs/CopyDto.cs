using MyShop.Models;
using System.ComponentModel.DataAnnotations;

namespace MyShop.DTOs
{
    public class CopyDto
    {
        public Dvdcopy Dvdcopy{ get; set; }

        [Required(ErrorMessage = "Member id is required!")]
        public int MemberId { get; set; }
        public int CopyId { get; set; } 
    }
}
