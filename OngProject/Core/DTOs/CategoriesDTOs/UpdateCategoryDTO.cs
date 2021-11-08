using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OngProject.Core.DTOs
{
    public class UpdateCategoryDTO
    {
        /// <summary>
        /// Name a modificar de Category
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description a modificar de Category
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Imagen a modificar de la Category. Solo se aceptan formatos:
        /// <br></br>
        /// .jpg , .jpng , .png
        /// </summary>
        public IFormFile Image { get; set; }
    }
}