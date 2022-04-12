using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyShop.Models
{
    [Table("DVDTitle")]
    public partial class Dvdtitle
    {
        public Dvdtitle()
        {
            DvDimages = new HashSet<DvDimage>();
            Dvdcopies = new HashSet<Dvdcopy>();
            Actors = new HashSet<Actor>();
        }

        [Key]
        public int DvdId { get; set; }
        public int ProduceId { get; set; }
        public int CategoryId { get; set; }
        public int StudioId { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateReleased { get; set; }
        [Column(TypeName = "decimal(28, 0)")]
        public decimal Rate { get; set; }
        [Column(TypeName = "decimal(28, 0)")]
        public decimal? PenaltyRate { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Dvdtitles")]
        public virtual Dvdcategory Category { get; set; }
        [ForeignKey("ProduceId")]
        [InverseProperty("Dvdtitles")]
        public virtual Producer Produce { get; set; }
        [ForeignKey("StudioId")]
        [InverseProperty("Dvdtitles")]
        public virtual Studio Studio { get; set; }
        [InverseProperty("DvDnumberNavigation")]
        public virtual ICollection<DvDimage> DvDimages { get; set; }
        [InverseProperty("Dvd")]
        public virtual ICollection<Dvdcopy> Dvdcopies { get; set; }

        [ForeignKey("DvdId")]
        [InverseProperty("Dvds")]
        public virtual ICollection<Actor> Actors { get; set; }
    }
}
