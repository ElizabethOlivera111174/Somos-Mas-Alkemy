using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OngProject.Core.Entities
{
    public class Organizations : EntityBase
    {
        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        [MaxLength(255)]
        public string Image { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        [MaxLength(255)]
        public string Address { get; set; }

        [Column(TypeName = "INTEGER")]
        [MaxLength(20)]
        public int Phone { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(320)")]
        [MaxLength(320)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "TEXT")]
        [MaxLength(500)]
        public string WelcomeText { get; set; }

        [Column(TypeName = "TEXT")]
        [MaxLength(2000)]
        public string AboutUsText { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        [MaxLength(255)]
        public string FacebookUrl { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        [MaxLength(255)]
        public string InstagramUrl { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        [MaxLength(255)]
        public string LinkedinUrl { get; set; }
    }
}

