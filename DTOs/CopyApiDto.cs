using System.ComponentModel.DataAnnotations;

namespace MyShop.DTOs
{
    public class CopyApiDto
    {
        [Required]
        public int CopyId { get; set; }

        [Required(ErrorMessage = "Please select a DvD/Movie that you're adding a copy of.")]
        public int DvdId { get; set; }

        [Required(ErrorMessage = "Please add the data of purchase.")]
        public DateTime DatePurchased { get; set; }

        [Required]
        [Range(1, 20)]
        public int TotalCopies { get; set; }
    }
}
