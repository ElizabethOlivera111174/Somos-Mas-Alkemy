using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Entities
{
    public class Comments : EntityBase
    {
        
        [Required]
        [Column(TypeName = "TEXT")]
        [MaxLength(4000)]
        public string Body { get; set; }

        [Required]
        [Column(TypeName = "INTEGER")]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        [Column(TypeName = "INTEGER")]
        public int NewId { get; set; }
        [ForeignKey("NewId")]
        public virtual News News { get; set; }

    }
}
