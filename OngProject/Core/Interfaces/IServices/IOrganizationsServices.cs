using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OngProject.Common;
using OngProject.Core.DTOs;

namespace OngProject.Core.Interfaces.IServices
{
    public interface IOrganizationsServices
    {
        Task<OrganizationsDTO> Get();
        Task <Result> Update(OrganizationUpdateDTO organizationUpdateDTO);
    }
}
