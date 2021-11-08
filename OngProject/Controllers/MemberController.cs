using Microsoft.AspNetCore.Authorization;
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
    public class MemberController : ControllerBase
    {
        #region Objects and Constructor
        private readonly IMemberServices _memberServices;
        public MemberController(IMemberServices memberServices)
        {
            _memberServices = memberServices;
        }
        #endregion

        #region Documentacion
        /// <summary>
        /// Endpoint para obtener todas las entradas de Miembros. Se integra una paginacion para optimizar
        /// y hacer mas performante la llamada. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// <para>
        /// Formato de peticion: https:// nombreDelServidor /member ?page =pagina que se consulta
        /// </para>
        /// <para>
        /// Ejemplo de peticion: https://localhost:44353/member?page=1
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
        [HttpGet()]
        public async Task<ActionResult<PaginationDTO<MembersDTO>>> GetAll([FromQuery] int page)
        {
            string route = Request.Path.Value.ToString();
            var response = await _memberServices.GetByPage(route, page);

            return Ok(response);
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para la creacion de un nuevo Miembro. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// Formato de peticion: <br></br>
        /// {
        ///     <br></br>
        ///     "name": "string",<br></br>
        ///     "facebookUrl": "string",<br></br>
        ///     "instagramUrl": "string",<br></br>
        ///     "linkedinUrl": "string",<br></br>
        ///     "image": "string",<br></br>
        ///     "description": "string"<br></br>
        /// }
        /// <br></br>
        /// Ejemplo de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "name": "MiembroPrueba",<br></br>
        ///     "facebookUrl": "https://www.facebook.com/MiembroPrueba",<br></br>
        ///     "instagramUrl": "https://www.instagram.com/MimebroPrueba",<br></br>
        ///     "linkedinUrl": "https://www.instagram.com/MiembroPrueba",<br></br>
        ///     "image": "https://cohorte-septiembre-91ddd87b.s3.amazonaws.com/Member_2510202114:44:13BS_Revenant_Grace.png",<br></br>
        ///     "description": "DescripcionPrueba",<br></br>
        /// }
        /// </remarks>
        /// <param name="membersDTO">DTO para la creacion de un nuevo Miembro</param>
        /// <returns>
        /// Falta formato para integrar clase Result
        /// </returns>
        /// <response code="200">El usuario se inserto con exito</response>
        /// <response code="400">No se ha podido procesar la solicitud con estos datos</response>
        /// <response code="401">Credenciales incorrectas</response>
        #endregion
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] MemberInsertDTO membersDTO)
        {
            if (!ModelState.IsValid) return BadRequest("Los datos ingresados son incorrectos");
            var response = await _memberServices.Insert(membersDTO);
            return Ok(response);
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para eliminacion de baja logica de un Miembro. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// <para>
        /// Formato de solicitud: https:// nombreDelServidor /member ?id=miembroAborrar
        /// </para>
        /// <para>
        /// Ejemplo de solicitud: https://localhost:44353/member?id=1
        /// </para>
        /// </remarks>
        /// <param name="id">Id del Miembro a borrarse, se recibe por solicitud</param>
        /// <returns>
        /// 
        /// </returns>
        /// <response code="200">Se ha eliminado al Miembro correctamente</response>
        /// <response code="401">Credenciales invalidas</response> 
        #endregion
        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            return Ok(await _memberServices.Delete(id));
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para la modificacion de Miembros. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <remarks>
        /// Formato de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "id": 0,<br></br>
        ///     "name": "string",<br></br>
        ///     "facebookUrl": "string",<br></br>
        ///     "instagramUrl": "string",<br></br>
        ///     "linkedinUrl": "string",<br></br>
        ///     "image": "string",<br></br>
        ///     "description": "string"<br></br>
        /// }
        /// <br></br>
        /// Ejemplo de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "name": "ModificacionMiembro",<br></br>
        ///     "facebookUrl": "https://www.facebook.com/ModificacionMiembro",<br></br>
        ///     "instagramUrl": "https://www.instagram.com/ModificacionMiembro",<br></br>
        ///     "linkedinUrl": "https://www.instagram.com/ModificacionMiembro",<br></br>
        ///     "image": "https://cohorte-septiembre-91ddd87b.s3.amazonaws.com/Member_2510202114:44:13BS_Revenant_Grace.png",<br></br>
        ///     "description": "DescripcionPrueba",<br></br>
        /// }
        /// </remarks>
        /// <param name="memberUpdateDTO">DTO para la modificacion de Miembros</param>
        /// <returns></returns>
        /// <response code="200">Solicitud ejecutada correctamente</response>
        /// <response code="400">No se ha podido completar la solicitud</response>
        /// <response code="401">Credenciales invalidas</response> 
        #endregion
        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public async Task<ActionResult<Result>> Update([FromForm] MemberUpdateDTO memberUpdateDTO)
        {
            var request = await _memberServices.Update(memberUpdateDTO);
            return request.HasErrors
                ? BadRequest(request.Messages) : Ok(request);

        }
    }
}
