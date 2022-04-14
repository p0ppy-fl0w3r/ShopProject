using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyShop.Models
{
    [Table("Studio")]
    public partial class Studio
    {
        public Studio()
        {
            Dvdtitles = new HashSet<Dvdtitle>();
        }

        [Key]
        public int StudioId { get; set; }
        
        [Required(ErrorMessage = "Please add the studio's name.")]
        public string StudioName { get; set; } = null!;

        [InverseProperty("Studio")]
        public virtual ICollection<Dvdtitle> Dvdtitles { get; set; }
    }
}
