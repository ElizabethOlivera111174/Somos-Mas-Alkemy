using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        #region Objects and Constructor
        private readonly ICommentsServices _commentsServices;
        public CommentsController(ICommentsServices commentsServices)
        {
            _commentsServices = commentsServices;
        }
        #endregion

          #region Documentacion
        /// <summary>
        /// Endpoint para obtener todas las entradas de Comentarios. Se integra una paginacion para optimizar
        /// y hacer mas performante la llamada. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// <para>
        /// Formato de peticion: https:// nombreDelServidor /coments ?page =pagina que se consulta
        /// </para>
        /// <para>
        /// Ejemplo de peticion: https://localhost:44353/coments?page=1
        /// </para>
        /// </remarks>
        /// <param name="page">
        /// Pagina que se pide a la Db. Si la pagina = 0, entonces regresa por defecto pagina: 1
        /// </param>
        /// <returns>
        /// Entradas de comentarios, con 10 entradas por pagina
        /// </returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response> 
        #endregion
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentsDTO>>> GetAll([FromQuery] int page)
        {
            string route = Request.Path.Value.ToString();
            var response = await _commentsServices.GetByPage(route, page);

            return Ok(response);
        }
         /// <summary>
        /// Endpoint para obtener una entrada de Comentarios determinada. Es publica
        /// </summary>
       
        /// <returns>
        /// Entradas de un comentario.
        /// </returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response> 
         
        [HttpGet("/comments/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!_commentsServices.EntityExists(id)) return NotFound();
            var contact = await _commentsServices.GetById(id);
            return Ok(contact);
        }

          #region Documentacion
        /// <summary>
        /// Endpoint para la creacion de un nuevo Comentario. Es publico
        /// </summary>
        /// <remarks>
        /// Formato de peticion: <br></br>
        /// {
        ///     <br></br>
        ///     "body": "string",<br></br>
        ///     "userId": 0,<br></br>
        ///     "newId": 0      
        ///  
        /// }
        /// <br></br>
        /// Ejemplo de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "body": "ejemplo Texto",<br></br>
        ///     "userId": 0,<br></br>
        ///     "newId": 0   
        ///     
        /// }
        /// </remarks>
        /// <param name="newCommentsDTO">DTO para la creacion de un nuevo comentario</param>
        /// <returns>
        /// Falta formato para integrar clase Result
        /// </returns>
        /// <response code="200">El usuario se inserto con exito</response>
        /// <response code="400">No se ha podido procesar la solicitud con estos datos</response>
        /// <response code="401">Credenciales incorrectas</response>
        #endregion

        [HttpPost]
        public async Task<IActionResult> Insert([FromForm] NewCommentsDTO newCommentsDTO)
        {
            var request = await _commentsServices.Insert(newCommentsDTO);

            return (request != null) ? Ok() : BadRequest("No se ha podido ingresar el comentario");
        }

          #region Documentacion
        /// <summary>
        /// Endpoint para eliminacion de baja logica de un Comentario. Se debe ser ADMINISTRADOR o USUARIO
        /// </summary>
        /// <remarks>
        /// <para>
        /// Formato de solicitud: https:// nombreDelServidor /comments ?id=miembroAborrar
        /// </para>
        /// <para>
        /// Ejemplo de solicitud: https://localhost:44353/comments?id=1
        /// </para>
        /// </remarks>
        /// <param name="id">Id del comentario a borrarse, se recibe por solicitud</param>
        /// <returns>
        /// 
        /// </returns>
        /// <response code="200">Se ha eliminado al comentario correctamente</response>
        /// <response code="401">Credenciales invalidas</response> 
        #endregion

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_commentsServices.EntityExists(id))
            {
                if (!await _commentsServices.ValidateCreatorOrAdmin(User, id)) return Forbid();
                return Ok(await _commentsServices.Delete(id));

            }
            return Ok(await _commentsServices.Delete(id));
        }

           #region Documentacion
        /// <summary>
        /// Endpoint para la modificacion de comentario. Se debe ser ADMINISTRADOR o USUARIO
        /// </summary>
        /// <remarks>
       /// Formato de peticion: <br></br>
        /// {
        ///     <br></br>
        ///     "body": "string",<br></br>
        ///     "userId": 0,<br></br>
        ///     "newId": 0      
        ///  
        /// }
        /// <br></br>
        /// Ejemplo de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "body": "ejemplo Texto",<br></br>
        ///     "userId": 0,<br></br>
        ///     "newId": 0   
        ///     
        /// }
        /// </remarks>
        /// <param name="CommentUpdateDTO">DTO para la modificacion de Miembros</param> 
        /// <param name="id">DTO para la modificacion de Miembros</param>        
        /// <returns></returns>
        /// <response code="200">Solicitud ejecutada correctamente</response>
        /// <response code="400">No se ha podido completar la solicitud</response>
        /// <response code="401">Credenciales invalidas</response> 
        #endregion

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] int id, [FromForm] CommentUpdateDTO CommentUpdateDTO )
        {
            if (_commentsServices.EntityExists(id))
            {
                if (!await _commentsServices.ValidateCreatorOrAdmin(User, id)) return Forbid();
                return Ok(await _commentsServices.Update(id, CommentUpdateDTO ));
            }
            return NotFound("El comentario no existe");
        }
    }
}
