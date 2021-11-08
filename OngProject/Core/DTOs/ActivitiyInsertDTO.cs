using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class ActivitiyInsertDTO
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El content es requerido")]
        public string Content { get; set; }
        [Required(ErrorMessage = "La imagen es requerida")]
        public IFormFile Image { get; set; }

    }
}
