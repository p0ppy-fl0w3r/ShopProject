using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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
        [StringLength(50)]
        [Required]
        public string ActorName { get; set; }
        [StringLength(50)]
        [Required]
        public string ActorLastName { get; set; }
        
        
        public string ProfileUrl { get; set; }

        [ForeignKey("ActorId")]
        [InverseProperty("Actors")]
        [JsonIgnore]
        public virtual ICollection<Dvdtitle> Dvds { get; set; }
    }
}
