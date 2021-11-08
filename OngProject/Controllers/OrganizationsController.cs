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
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationsServices _organizationsServices;

        public OrganizationsController(IOrganizationsServices organizationsServices)
        {
            _organizationsServices = organizationsServices;
        }

        [HttpGet("/public")]
        public async Task<OrganizationsDTO> Get()
        {
            return await _organizationsServices.Get();
        }
        
        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public async Task<ActionResult<Result>> Update ([FromForm] OrganizationUpdateDTO organizationUpdateDTO)
        {
            var request= await _organizationsServices.Update(organizationUpdateDTO);
            return request.HasErrors
                ? BadRequest(request.Messages):Ok(request);

        }
    }
}
