using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs.SlidesDTOs
{
    public class UpdateSlideDTO
    {
        public IFormFile ImageUrl { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public int OrganizationId { get; set; }
    }
}
