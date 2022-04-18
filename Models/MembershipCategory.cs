using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyShop.Models
{
    [Table("MembershipCategory")]
    public partial class MembershipCategory
    {
        public MembershipCategory()
        {
            Members = new HashSet<Member>();
        }

        [Key]
        public int MemberCategoryId { get; set; }
        
        [Required]
        public string Description { get; set; }
        public int TotalLoans { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Member> Members { get; set; }
    }
}
