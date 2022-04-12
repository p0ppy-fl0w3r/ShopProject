using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyShop.Models
{
    [Table("Actor")]
    public partial class Actor
    {
        public Actor()
        {
            Dvds = new HashSet<Dvdtitle>();
        }

        [Key]
        public int ActorId { get; set; }
        [Required]
        [StringLength(50)]
        public string ActorName { get; set; }
        [Required]
        [StringLength(50)]
        
        public string ActorLastName { get; set; }
        
        
        public string ProfileUrl { get; set; }

        [ForeignKey("ActorId")]
        [InverseProperty("Actors")]
        public virtual ICollection<Dvdtitle> Dvds { get; set; }
    }
}
