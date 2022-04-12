using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyShop.Models
{
    [Table("DVDCategory")]
    public partial class Dvdcategory
    {
        public Dvdcategory()
        {
            Dvdtitles = new HashSet<Dvdtitle>();
        }

        [Key]
        public int CategoryId { get; set; }
        
        
        public string CategoryDescription { get; set; }
        [Required]
        public string AgeRating { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Dvdtitle> Dvdtitles { get; set; }
    }
}
