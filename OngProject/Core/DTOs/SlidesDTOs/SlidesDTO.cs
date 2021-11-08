using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class SlidesDTO
    {
        public string ImageUrl { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public int OrganizationId { get; set; }

    }
}
