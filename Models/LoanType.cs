using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyShop.Models
{
    [Table("LoanType")]
    public partial class LoanType
    {
        public LoanType()
        {
            Loans = new HashSet<Loan>();
        }

        [Key]
        public int LoanTypeId { get; set; }

        [Required]
        public string LoanTypeName { get; set; }
        [Required]
        public int? DurationDays { get; set; }

        [InverseProperty("Type")]
        public virtual ICollection<Loan> Loans { get; set; }
    }
}
