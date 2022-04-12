using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyShop.Models
{
    [Table("Member")]
    public partial class Member
    {
        public Member()
        {
            Loans = new HashSet<Loan>();
        }

        [Key]
        public int MemberId { get; set; }
        public int CategoryId { get; set; }
        [Required]
        
        
        public string Address { get; set; }
        [Required]
        [StringLength(50)]
        
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        
        public string LastName { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        
        
        public string MemberImage { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Members")]
        public virtual MembershipCategory Category { get; set; }
        [InverseProperty("Member")]
        public virtual ICollection<Loan> Loans { get; set; }
    }
}
