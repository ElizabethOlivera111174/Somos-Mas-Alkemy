using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestimonialsController : ControllerBase
    {
        private readonly ITestimonialsServices _testimonialsServices;
        public TestimonialsController(ITestimonialsServices testimonialsServices)
        {
            _testimonialsServices = testimonialsServices;
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para obtener todas las entradas de Testimonials. Se integra una paginacion para optimizar
        /// y hacer mas performante la llamada. Se debe estar logueado
        /// </summary>
        /// <remarks>
        /// <para>
        /// Formato de peticion: https:// nombreDelServidor /testimonials ?page =pagina que se consulta
        /// </para>
        /// <para>
        /// Ejemplo de peticion: https://localhost:44353/testimonials?page=1
        /// </para>
        /// </remarks>
        /// <param name="page">
        /// Pagina que se pide a la Db. Si la pagina = 0, entonces regresa por defecto pagina: 1
        /// </param>
        /// <returns>
        /// Entradas de testimonios, con 10 entradas por pagina
        /// </returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response> 
        /// <response code="204">Pagina sin contenido</response> 
        #endregion
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PaginationDTO<TestimonialsCreateDTO>>> GetAll([FromQuery] int page)
        {
            try
            {
                string route = Request.Path.Value.ToString();
                var response = await _testimonialsServices.GetByPage(route, page);

                return Ok(response);
            }
            catch (Exception result)
            {
                return BadRequest(result.Message);
            }
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para la creacion de un nuevo testimonio. Es necesario ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// Formato de peticion: <br></br>
        /// {
        ///     <br></br>
        ///     "name": "string",<br></br>
        ///     "content": "string",<br></br>
        ///     "image": File      
        ///  
        /// }
        /// <br></br>
        /// Ejemplo de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "name": "Testimonial prueba name",<br></br>
        ///     "content": "Testimonial prueba contenido",<br></br>
        ///     "image": File   
        ///     
        /// }
        /// </remarks>
        /// <param name="testimonialDTO">DTO para la creacion de un nuevo testimonio</param>
        /// <response code="200">El testimonio se inserto con exito</response>
        /// <response code="400">No se ha podido procesar la solicitud con estos datos</response>
        /// <response code="401">Credenciales incorrectas</response>
        #endregion
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<Result>> Insert([FromForm] TestimonialsCreateDTO testimonialDTO)
        {
            try
            {
                var response = await _testimonialsServices.Insert(testimonialDTO);

                return Ok(response);
            }
            catch (Exception result)
            {
                return BadRequest(result.Message);
            }
        }
        #region Documentacion
        /// <summary>
        /// Endpoint para eliminacion de baja logica de un testimonio. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// <para>
        /// Formato de solicitud: https:// nombreDelServidor /testimonials ?id=testimonialAborrar
        /// </para>
        /// <para>
        /// Ejemplo de solicitud: https://localhost:44353/testimonials?id=1
        /// </para>
        /// </remarks>
        /// <param name="id">Id del testimonials a borrarse, se recibe por solicitud</param>
        /// <returns>
        /// 
        /// </returns>
        /// <response code="200">Se ha eliminado al testimonial correctamente</response>
        /// <response code="401">Credenciales invalidas</response> 
        /// <response code="404">No se ha encontrado el testimonial</response> 
        #endregion
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            try
            {
                var response = await _testimonialsServices.Delete(id);

                return Ok(response);
            }
            catch (Exception result)
            {
                return BadRequest(result.Message);
            }
        }
        #region Documentacion
        /// <summary>
        /// Endpoint para la modificacion de testimonials. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// Formato de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "id": 0,<br></br>
        ///     "name": "string",<br></br>
        ///     "content": "string",<br></br>
        ///     "image": File,<br></br>
        /// }
        /// <br></br>
        /// Ejemplo de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "name": "ModificacionTestimonio",<br></br>
        ///     "content": "Nuevo contenido",<br></br>
        ///     "image": File,<br></br>
        /// }
        /// </remarks>
        /// <param name="testimonialsUpdateDTO">DTO para la modificacion de testimonios</param>
        /// <param name="id">Id del testimonial que se quiere modificar</param>
        /// <returns></returns>
        /// <response code="200">Solicitud ejecutada correctamente</response>
        /// <response code="401">Credenciales invalidas</response> 
        #endregion
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Result>> Update(int id, [FromForm] TestimonialsUpdateDTO testimonialsUpdateDTO)
        {
            try
            {
                var response = await _testimonialsServices.Update(id, testimonialsUpdateDTO);
                return Ok(response);
            }
            catch (Exception result) { return BadRequest(result.Message); }
        }
    }
}
