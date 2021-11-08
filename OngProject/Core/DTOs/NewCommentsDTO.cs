using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.DTOs
{
    public class NewCommentsDTO
    {
        /// <summary>
        /// Comantario a crear
        /// </summary>
        [Required]
        public string Body { get; set; }
        /// <summary>
        /// Id del usuario
        /// </summary>
        [Required]
        public int UserId { get; set; }
        /// <summary>
        /// Id de la Noticia a modificar
        /// </summary>
        [Required]
        public int NewId { get; set; }
    }
}