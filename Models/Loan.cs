using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyShop.Models
{
    [Table("Loan")]
    public partial class Loan
    {
        [Key]
        public int LoanId { get; set; }
        public int CopyId { get; set; }
        public int MemberId { get; set; }
        public int TypeId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateOut { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateDue { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ReturnedDate { get; set; }

        [ForeignKey("CopyId")]
        [InverseProperty("Loans")]
        public virtual Dvdcopy Copy { get; set; }
        [ForeignKey("MemberId")]
        [InverseProperty("Loans")]
        public virtual Member Member { get; set; }
        [ForeignKey("TypeId")]
        [InverseProperty("Loans")]
        public virtual LoanType Type { get; set; }
    }
}
