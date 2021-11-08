using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OngProject.Core.Entities
{
    public abstract class EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        #region 
        /// <summary>
        /// IsDeleted para borrado de baja logica, donde: 
        /// true = borrado
        /// false = Activo
        /// </summary> 
        #endregion
        [JsonIgnore]
        public bool IsDeleted { get; set; }

        #region 
        /// <summary>
        /// Para indicar fechas de modificaciones en al Db. 
        /// </summary> 
        #endregion
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}
