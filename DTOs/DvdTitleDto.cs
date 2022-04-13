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

        public decimal Rate { get; set; }

        public decimal? PenaltyRate { get; set; }
    }
}
