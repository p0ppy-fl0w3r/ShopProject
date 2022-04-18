using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyShop.Models
{
    [Table("Producer")]
    public partial class Producer
    {
        public Producer()
        {
            Dvdtitles = new HashSet<Dvdtitle>();
        }

        [Key]
        public int ProducerId { get; set; }
        [Required]
        [StringLength(75)]
        
        public string ProducerName { get; set; }

        [InverseProperty("Produce")]
        public virtual ICollection<Dvdtitle> Dvdtitles { get; set; }
    }
}
