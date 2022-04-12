using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyShop.Models
{
    [Table("DvDImages")]
    public partial class DvDimage
    {
        [Key]
        [Column("DvDImageId")]
        public int DvDimageId { get; set; }
        public int DvDnumber { get; set; }
        [Column("DvdImage")]
        
        
        public string DvdImage1 { get; set; }

        [ForeignKey("DvDnumber")]
        [InverseProperty("DvDimages")]
        public virtual Dvdtitle DvDnumberNavigation { get; set; }
    }
}
