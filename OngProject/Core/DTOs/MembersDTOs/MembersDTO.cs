using System;
using System.ComponentModel.DataAnnotations;


namespace OngProject.Core.DTOs
{
    public class MembersDTO
    {
        [Required(ErrorMessage = "Escriba su nombre")]
        public string Name { get; set; }
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string LinkedinUrl { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}
