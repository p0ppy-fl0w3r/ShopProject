using MyShop.Models;
using System.ComponentModel.DataAnnotations;

namespace MyShop.DTOs
{
    public class DvdTitleDto
    {
        public Dvdcategory Category { get; set; }

        public Producer Producer { get; set; }

        public Studio Studio { get; set; }

        public DateTime DateReleased { get; set; }

        [Required(ErrorMessage = "Please add a DvD title.")]
        [MaxLength(100)]
        public string Title { get; set; }   

        public decimal Rate { get; set; }

        public decimal? PenaltyRate { get; set; }

        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Please choose at least one image.")]
        public IFormFileCollection DvDImages { get; set; }


        public List<Actor> Actors { get; set; }


    }
}
