using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyShop.Models
{
    [Table("DVDCopy")]
    public partial class Dvdcopy
    {
        public Dvdcopy()
        {
            Loans = new HashSet<Loan>();
        }

        [Key]
        public int CopyId { get; set; }
        public int DvdId { get; set; }
        [Column(TypeName = "date")]
        public DateTime DatePurchased { get; set; }

        [ForeignKey("DvdId")]
        [InverseProperty("Dvdcopies")]
        public virtual Dvdtitle Dvd { get; set; }
        [InverseProperty("Copy")]
        public virtual ICollection<Loan> Loans { get; set; }
    }
}
