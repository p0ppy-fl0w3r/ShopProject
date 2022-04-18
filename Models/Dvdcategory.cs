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
        
        [Required]
        public string CategoryDescription { get; set; }
        public bool AgeRestricted { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Dvdtitle> Dvdtitles { get; set; }
    }
}
