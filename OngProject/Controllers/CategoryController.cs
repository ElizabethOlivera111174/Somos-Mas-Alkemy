using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
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
    public class CategoryController : ControllerBase
    {
        #region Object and Constructor
        private readonly ICategoriesServices _CategoriesServices;
        public CategoryController(ICategoriesServices CategoriesServices)
        {
            _CategoriesServices = CategoriesServices;
        }
        #endregion
        #region Documentacion
        /// <summary>
        /// Endpoint para obtener todas las entradas de Categories. Se integra una paginacion para optimizar
        /// y hacer mas performante la llamada. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// <para>
        /// Formato de peticion: https:// nombreDelServidor /Category ?page =pagina que se consulta
        /// </para>
        /// <para>
        /// Ejemplo de peticion: https://localhost:44353/Category?page=1
        /// </para>
        /// </remarks>
        /// <param name="page">
        /// Pagina que se pide a la Db. Si la pagina = 0, entonces regresa por defecto pagina: 1
        /// </param>
        /// <returns>
        /// Entradas de miembros, con 10 entradas por pagina
        /// </returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response> 
        #endregion
        [Authorize(Roles = "Administrator")]
        [HttpGet("/GetAll")]
        public async Task<ActionResult<IEnumerable<CategoryNameDTO>>> GetAll([FromQuery] int page)
        {
            string route = Request.Path.Value.ToString();
            var response = await _CategoriesServices.GetByPage(route, page);

            return Ok(response);
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para la obtención de una Category. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// <para>
        /// Formato de solicitud: https:// nombreDelServidor /Category ?id=categoryaBuscar
        /// </para>
        /// <para>
        /// Ejemplo de solicitud: https://localhost:44353/Category?id=1
        /// </para>
        /// </remarks>
        /// <param name="id">Id de la Category a buscar, se recibe por solicitud</param>
        /// <returns>
        /// 
        /// </returns>
        /// <response code="200">Se ha eliminado la Category correctamente</response>
        /// <response code="401">Credenciales invalidas</response> 
        #endregion
        [Authorize(Roles = "Administrator")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!_CategoriesServices.EntityExist(id)) return NotFound();
            var category = await _CategoriesServices.GetById(id);
            return Ok(category);
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para la creacion de un nueva Category. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// Formato de peticion: <br></br>
        /// {
        ///     <br></br>
        ///     "name": "string",<br></br>
        ///     "description": "string",<br></br>
        ///     "image": "IFormFile",<br></br>
        /// }
        /// <br></br>
        /// Ejemplo de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "name": "CategoryPrueba",<br></br>
        ///     "image": "https://cohorte-septiembre-91ddd87b.s3.amazonaws.com/Category_2510202114:44:13BS_Revenant_Grace.png",<br></br>
        ///     "description": "DescripcionPrueba",<br></br>
        /// }
        /// </remarks>
        /// <param name="categoryDTO">DTO para la creacion de una nueva Category</param>
        /// <response code="200">LA  Category se inserto con exito</response>
        /// <response code="400">No se ha podido procesar la solicitud con estos datos</response>
        /// <response code="401">Credenciales incorrectas</response>
        #endregion
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid) return BadRequest();
            var response = await _CategoriesServices.Post(categoryDTO);
            return CreatedAtAction("POST", response);
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para la modificacion de Categories. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// Formato de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "id": 0,<br></br>
        ///     "name": "string",<br></br>
        ///     "image": "IFormFile",<br></br>
        ///     "description": "string"<br></br>
        /// }
        /// <br></br>
        /// Ejemplo de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "name": "ModificacionCategory",<br></br>
        ///     "image": "https://cohorte-septiembre-91ddd87b.s3.amazonaws.com/Category_2510202114:44:13BS_Revenant_Grace.png",<br></br>
        ///     "description": "DescripcionPrueba",<br></br>
        /// }
        /// </remarks>
        /// <param name="updateCategoryDTO">DTO para la modificacion de Category</param>
        /// <returns></returns>
        /// <response code="200">Solicitud ejecutada correctamente</response>
        /// <response code="400">No se ha podido completar la solicitud</response>
        /// <response code="401">Credenciales invalidas</response> 
        #endregion
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Result>> Update(int id, [FromForm] UpdateCategoryDTO updateCategoryDTO)
        {
            var request = await _CategoriesServices.Update(id, updateCategoryDTO);
            
            return request.HasErrors
                ? BadRequest(request.Messages)
                : Ok(request);
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para eliminacion de baja logica de una Category. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// <para>
        /// Formato de solicitud: https:// nombreDelServidor /Category ?id=categoryAborrar
        /// </para>
        /// <para>
        /// Ejemplo de solicitud: https://localhost:44353/Category?id=1
        /// </para>
        /// </remarks>
        /// <param name="id">Id de la Category a borrarse, se recibe por solicitud</param>
        /// <returns>
        /// 
        /// </returns>
        /// <response code="200">Se ha eliminado la Category correctamente</response>
        /// <response code="401">Credenciales invalidas</response> 
        #endregion
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            var request = await _CategoriesServices.Delete(id);

            return request.HasErrors
                ? BadRequest(request.Messages)
                : Ok(request);
        }

    }
}
