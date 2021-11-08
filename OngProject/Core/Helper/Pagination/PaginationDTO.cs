using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OngProject.Core.Helper.Pagination
{
    public class PaginationDTO<T>
    {
        /// <summary>
        /// Pagina que se esta visualizando
        /// </summary>
        public int CurrentPage { get; init; }
        
        /// <summary>
        /// Numero total de entradas que hay en la Db
        /// </summary>
        public int TotalItems { get; init; }
        
        /// <summary>
        /// Numero total de paginas en Db. Cambia con respecto a la cantidad de 
        /// elementos por pagina que se piden (por default, =10)
        /// </summary>
        public int TotalPages { get; init; }
        
        /// <summary>
        /// Url con la pagina previa
        /// </summary>
        public string PrevPage { get; init; }
        
        /// <summary>
        /// Url con la pagina siguiente
        /// </summary>
        public string NextPage { get; init; }

        /// <summary>
        /// Lista de elementos por pagina
        /// </summary>
        public List<T> Items { get; init; }
    }
}
