using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {

        private readonly INewsServices _newsServices;

        public NewsController(INewsServices newsServices)
        {
            _newsServices = newsServices;
        }

        /// <summary>
        /// Endpoint para consultar una Novedad por su id.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Formato de solicitud: https:// nombreDelServidor /News ?id=IdNovedadAConsultar
        /// </para>
        /// <para>
        /// Ejemplo de solicitud: https://localhost:44353/News?id=1
        /// </para>
        /// </remarks>
        /// <param name="id">Id de la Novedad a consultar, se recibe por parametro</param>
        /// <returns>
        /// El resultado de la peticion
        /// </returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales invalidas</response>
        /// <response code="400">No se ha podido procesar la solicitud con estos datos</response>

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (!_newsServices.EntityExists(id))
                    return NotFound();
                var news = await _newsServices.GetById(id);
                return Ok(news);
            }
            catch(Exception)
            {
                return NotFound();
            }
            
        }

        /// <summary>
        /// Endpoint para obtener todas las Novedades. Se integra una paginacion para optimizar
        /// y hacer mas performante la llamada.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Formato de peticion: https:// nombreDelServidor /News ?page =pagina que se consulta
        /// </para>
        /// <para>
        /// Ejemplo de peticion: https://localhost:44353/News?page=1
        /// </para>
        /// </remarks>
        /// <param name="page">
        /// Pagina que se pide a la Db. Si la pagina = 0, entonces regresa por defecto pagina: 1
        /// </param>
        /// <returns>
        /// Entradas de novedades, con 10 entradas por pagina
        /// </returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response>
        /// <response code="404">Novedad no encontrada</response>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PaginationDTO<NewsDTO>>> GetAll([FromQuery] int page)
        {
            try
            {
                string route = Request.Path.Value.ToString();
                var response = await _newsServices.GetByPage(route, page);

                return Ok(response);
            }
            catch(Exception)
            {
                return NotFound();
            }
            
        }

        /// <summary>
        /// Endpoint para la creacion de una nueva Novedad. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// Formato de peticion: <br></br>
        /// {
        ///     <br></br>
        ///     "name": "string",<br></br>
        ///     "content": "string",<br></br>
        ///     "image": "IFormFile",<br></br>
        ///     "categoriaId": "int"<br></br>
        /// }
        /// <br></br>
        /// Ejemplo de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "name": "NovedadPrueba",<br></br>
        ///     "content": "contenidoNovedad",<br></br>
        ///     "image": "https://cohorte-septiembre-91ddd87b.s3.amazonaws.com/NuevaImagen.jpg",<br></br>
        ///     "categoriaId": "Numero de Id de la categoria a la que pertenece",<br></br>
        /// }
        /// </remarks>
        /// <param name="newsInsertDTO">DTO para la creacion de una nueva Novedad</param>
        /// <returns>
        /// El resultado de la peticion
        /// </returns>
        /// <response code="200">La Novedad se inserto con exito</response>
        /// <response code="400">No se ha podido procesar la solicitud con estos datos</response>
        /// <response code="401">Credenciales incorrectas</response>
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Insert([FromForm]NewsInsertDTO newsInsertDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                Result response = await _newsServices.Insert(newsInsertDTO);
                return (response != null) ? Ok("Novedad creada con exito") : BadRequest("Ocurrio un error al crear la Novedad");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
             
        }

        /// <summary>
        /// Endpoint para eliminacion de baja logica de una Novedad. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// <para>
        /// Formato de solicitud: https:// nombreDelServidor /News ?id=IdNovedadAborrar
        /// </para>
        /// <para>
        /// Ejemplo de solicitud: https://localhost:44353/News?id=1
        /// </para>
        /// </remarks>
        /// <param name="id">Id de la Novedad a borrar, se recibe por parametro</param>
        /// <returns>
        /// El resultado de la peticion
        /// </returns>
        /// <response code="200">Se ha eliminado la Novedad correctamente</response>
        /// <response code="401">Credenciales invalidas</response>
        /// <response code="400">No se ha podido procesar la solicitud con estos datos</response>
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            try
            {
                Result request = await _newsServices.Delete(id);
                return request.HasErrors ? BadRequest(request.Messages) : Ok(request);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Endpoint para la modificacion de una nueva Novedad. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// Formato de peticion: <br></br>
        /// {
        ///     <br></br>
        ///     "name": "string",<br></br>
        ///     "content": "string",<br></br>
        ///     "image": "IFormFile",<br></br>
        ///     "categoriaId": "int"<br></br>
        /// }
        /// <br></br>
        /// Ejemplo de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "name": "nombreNuevo",<br></br>
        ///     "content": "contenidoNuevo",<br></br>
        ///     "image": "https://cohorte-septiembre-91ddd87b.s3.amazonaws.com/NuevaImagen.jpg",<br></br>
        ///     "categoriaId": "Numero de Id de la categoria a la que pertenece",<br></br>
        /// }
        /// </remarks>
        /// <param name="id">Id de la Novedad a modificar</param>
        /// <param name="newsUpdateDTO">DTO para la modificacion de una nueva Novedad</param>
        /// <returns>
        /// El resultado de la peticion
        /// </returns>
        /// <response code="200">La Novedad se inserto con exito</response>
        /// <response code="400">No se ha podido procesar la solicitud con estos datos</response>
        /// <response code="401">Credenciales incorrectas</response>
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Result>> Update([FromForm] NewsUpdateDTO newsUpdateDTO, int id)
        {
            try
            {
                Result request = await _newsServices.Update(newsUpdateDTO, id);
                return request.HasErrors ? BadRequest(request.Messages) : Ok(request);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
