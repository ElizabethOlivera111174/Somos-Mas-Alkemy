using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.DTOs
{
    public class MemberUpdateDTO
    {
        /// <summary>
        /// Id del usuario a modificar
        /// </summary>
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Nombre a modificar
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url de Facebook a modificar
        /// </summary>
        [Url]
        public string FacebookUrl { get; set; }

        /// <summary>
        /// Url de Instagram a modificar
        /// </summary>
        [Url]
        public string InstagramUrl { get; set; }

        /// <summary>
        /// Url de LinkedIn a modificar
        /// </summary>
        [Url]
        public string LinkedinUrl { get; set; }

        /// <summary>
        /// Imagen a modificar del miembro. Solo se aceptan formatos:
        /// <br></br>
        /// .jpg , .jpng , .png
        /// </summary>
        public IFormFile Image { get; set; }

        /// <summary>
        /// Descripcion del miembro a modificar
        /// </summary>
        public string Description { get; set; }
    }
}
