using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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

        [Required(ErrorMessage = "Please select a DvD/Movie that you're adding a copy of.")]
        public int DvdId { get; set; }
        [Column(TypeName = "date")]
        [Required(ErrorMessage = "Please add the data of purchase.")]
        public DateTime DatePurchased { get; set; }

        [ForeignKey("DvdId")]
        [InverseProperty("Dvdcopies")]
        [JsonIgnore]
        public virtual Dvdtitle Dvd { get; set; } = null!;
        [InverseProperty("Copy")]
        [JsonIgnore]
        public virtual ICollection<Loan> Loans { get; set; }
    }
}
