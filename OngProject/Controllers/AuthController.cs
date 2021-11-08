using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using OngProject.Core.Mapper;
using OngProject.Core.Helper;
using OngProject.Core.Services;
using NSwag.Annotations;
using OngProject.Core.DTOs.UserDTOs;

namespace OngProject.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Object and Constructor
        private readonly IUserServices _userServices;
        private readonly JwtHelper _JwtHelper;
        public AuthController(IUserServices _userServices, JwtHelper _JwtHelper)
        {
            this._userServices = _userServices;
            this._JwtHelper = _JwtHelper;
        }
        #endregion

        #region Documentacion
        /// <summary>
        /// Endpoint para obtener los datos almacenados del usuario logueado
        /// </summary>
        /// <remarks>
        /// <para>
        /// Formato de peticion: https:// nombreDelServidor /auth/me
        /// </para>
        /// </remarks>
        /// <returns>
        /// FirstName , LastName ,Email ,Photo, RoleId 
        /// </returns>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="400">No pudo retornar los datos del usuario</response> 
        /// <response code="401">Credenciales incorrectas</response>

        #endregion
        [HttpGet("me")]
        public async Task<ActionResult> Get()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("Datos no validos");
                var token = Request.Headers["Authorization"];
                var userId = _userServices.GetUserId(token);
                var user = await _userServices.GetById(userId);
                return Ok(user);

            }
            catch (Exception result)
            {
                return BadRequest(result.Message);
            }
        }
        #region Documentacion
        /// <summary>
        /// Endpoint para el registro de un usuario en el sistema
        /// </summary>
        /// <remarks>
        /// Formato de peticion: <br></br>
        /// {
        ///     <br></br>
        ///     "FirstName*": "string",<br></br>
        ///     "LastName*": "string",<br></br>
        ///     "Email*": "string",<br></br>
        ///     "Password*": "string"<br></br>
        /// }
        /// <br></br>
        /// Ejemplo de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "FirstName*": "Alkemy",<br></br>
        ///     "LastName*": "Aceleracion",<br></br>
        ///     "Email*": "user@example.com",<br></br>
        ///     "Password*": "alkemy1234"<br></br>
        /// }
        /// </remarks>
        /// <param name="UserInsertDTO">DTO para la creacion de un nuevo Miembro</param>
        /// <returns>
        ///     Devuelve el token del usuario en caso de ser creado o Response indicando el error
        /// </returns>
        /// <response code="200">El usuario se inserto con exito</response>
        /// <response code="400">No se ha podido procesar la solicitud con estos datos</response>
        /// <response code="401">Credenciales incorrectas</response>
        #endregion
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] UserInsertDTO userInsertDTO)
        {
            try
            {
                if (ModelState.IsValid)
                    if (await _userServices.GetByEmail(userInsertDTO.Email) == null)
                        await _userServices.Register(userInsertDTO);
                    else
                        return BadRequest("Este correo ya estaba en uso");
                else
                    return BadRequest("Datos invalidos, verifique los campos ingresados");
                
                return Ok(new
                {
                    Status = "Operacion exitosa! Usuario creado con exito!",
                    Token = (OkObjectResult)await Login(new LoginDTO
                    {
                        Email = userInsertDTO.Email,
                        Password = userInsertDTO.Password
                    }
                    )
                });
            }
            catch (Exception result)
            {
                return BadRequest(result.Message);
            }
        }
        #region Documentacion
        /// <summary>
        /// Endpoint para el login de un usuario en el sistema
        /// </summary>
        /// <remarks>
        /// Formato de peticion: <br></br>
        /// {
        ///     <br></br>
        ///     "Email": "string",<br></br>
        ///     "Password": "string"<br></br>
        /// }
        /// <br></br>
        /// Ejemplo de solicitud: <br></br>
        /// {
        ///     <br></br>
        ///     "Email": "user@example.com",<br></br>
        ///     "Password": "alkemy1234"<br></br>
        /// }
        /// </remarks>
        /// <param name="LoginDTO">DTO para la creacion de un nuevo Miembro</param>
        /// <returns>
        ///     Devuelve el token del usuario en caso de que las credenciales sean correctas o Response indicando el error
        /// </returns>
        /// <response code="200">El usuario se logueo con exito</response>
        /// <response code="400">No se ha podido procesar la solicitud con estos datos</response>
        /// <response code="401">Credenciales incorrectas</response>
        #endregion
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromForm] LoginDTO loginDTO)
        {
            try
            {
                var userSaved = await _userServices.GetByEmail(loginDTO.Email);

                if (userSaved == null)
                {
                    return BadRequest("No se ha encontrado un usuario con este correo");
                }

                var passwordVerification = Encrypt.Verify(loginDTO.Password, userSaved.Password);
                if (!passwordVerification)
                {
                    return StatusCode(401, "Credenciales no validas");
                }

                var token = _JwtHelper.GenerateJwtToken(userSaved);

                return Ok(token);
            }
            catch (Exception result)
            {
                return BadRequest(result.Message);
            }
        }
    }
}
