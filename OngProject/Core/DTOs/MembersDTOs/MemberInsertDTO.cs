using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace OngProject.Core.DTOs
{
    public class MemberInsertDTO
    {

        /// <summary>
        /// Requerido: Nombre del nuevo miembro
        /// </summary>
        [Required(ErrorMessage = "Escriba su nombre")]
        public string Name { get; set; }

        /// <summary>
        /// Url del Facebook del nuevo miembro
        /// </summary>
        [Url]
        public string FacebookUrl { get; set; }

        /// <summary>
        /// Url del Instagram del nuevo miembro
        /// </summary>
        [Url]
        public string InstagramUrl { get; set; }

        /// <summary>
        /// Url del Linkdn del nuevo miembro
        /// </summary>
        [Url]
        public string LinkedinUrl { get; set; }

        /// <summary>
        /// Imagen del nuevo miembro. Solo se aceptan formatos:
        /// .jpg , .jpng , .png
        /// </summary>
        public IFormFile Image { get; set; }

        /// <summary>
        /// Descripcion del nuevo miembro
        /// </summary>
        public string Description { get; set; }
    }
}
