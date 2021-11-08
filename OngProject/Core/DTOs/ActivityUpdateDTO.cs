using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class ActivityUpdateDTO
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public IFormFile Image { get; set; }
    }
}
