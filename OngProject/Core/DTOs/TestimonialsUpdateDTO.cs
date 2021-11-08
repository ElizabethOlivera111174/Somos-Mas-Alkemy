using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class TestimonialsUpdateDTO
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public IFormFile Imagen { get; set; }
    }
}
