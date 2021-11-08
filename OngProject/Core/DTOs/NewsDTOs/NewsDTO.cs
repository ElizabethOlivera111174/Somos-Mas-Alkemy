using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.DTOs
{
    public class NewsDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
