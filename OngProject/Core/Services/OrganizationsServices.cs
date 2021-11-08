using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Mapper;
using OngProject.Core.Services.AWS;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class OrganizationsServices : IOrganizationsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _ImageService;

        public OrganizationsServices(IUnitOfWork unitOfWork, IImageService ImageService)
        {
            _unitOfWork = unitOfWork;
            _ImageService = ImageService;
           
        }

        public async Task<OrganizationsDTO> Get()
        {
            var mapper = new EntityMapper();
            var organization = await _unitOfWork.OrganizationsRepository.GetAll();
            var organizationDTO = mapper.FromOrganizationToOrganizationDto(organization.First());
            return organizationDTO;
        }

        public async Task<Result> Update(OrganizationUpdateDTO organizationUpdateDTO)
        {
            var consulta= await _unitOfWork.OrganizationsRepository.GetById(organizationUpdateDTO.Id);

            if(consulta==null)
            {
                return new Result().NotFound();
            }

            if(organizationUpdateDTO.Image!= null)
            {
            if(consulta.Image != organizationUpdateDTO.Image.ToString())
            {
                try
                {
                    await _ImageService.Delete(consulta.Image);
                }
                catch (System.Exception)
                {
                }  
                string uniqueName = "Organization_" + DateTime.Now.ToString().Replace(",", "").Replace("/", "").Replace(" ", "");
                var urlImage = await _ImageService.Save(uniqueName + organizationUpdateDTO.Image.FileName, organizationUpdateDTO.Image);
                consulta.Image = urlImage;                      
            }
            }
        
            consulta.Name= (consulta.Name == organizationUpdateDTO.Name || string.IsNullOrEmpty(organizationUpdateDTO.Name))? consulta.Name : organizationUpdateDTO.Name;
            consulta.Address=(consulta.Address == organizationUpdateDTO.Address || string.IsNullOrEmpty(organizationUpdateDTO.Address))? consulta.Address : organizationUpdateDTO.Address;
            consulta.Phone=(consulta.Phone == organizationUpdateDTO.Phone || organizationUpdateDTO.Phone == 0)? consulta.Phone : organizationUpdateDTO.Phone;  
            consulta.FacebookUrl= (consulta.FacebookUrl == organizationUpdateDTO.FacebookUrl || string.IsNullOrEmpty(organizationUpdateDTO.FacebookUrl))? consulta.FacebookUrl : organizationUpdateDTO.FacebookUrl;
            consulta.Email= (consulta.Email == organizationUpdateDTO.Email || string.IsNullOrEmpty(organizationUpdateDTO.Email))? consulta.Email : organizationUpdateDTO.Email;
            consulta.WelcomeText= (consulta.WelcomeText == organizationUpdateDTO.WelcomeText || string.IsNullOrEmpty(organizationUpdateDTO.WelcomeText))? consulta.WelcomeText : organizationUpdateDTO.WelcomeText;
            consulta.AboutUsText= (consulta.AboutUsText == organizationUpdateDTO.AboutUsText || string.IsNullOrEmpty(organizationUpdateDTO.AboutUsText))? consulta.AboutUsText : organizationUpdateDTO.AboutUsText;
            consulta.InstagramUrl= (consulta.InstagramUrl == organizationUpdateDTO.InstagramUrl || string.IsNullOrEmpty(organizationUpdateDTO.InstagramUrl))? consulta.InstagramUrl : organizationUpdateDTO.InstagramUrl;
            consulta.LinkedinUrl= (consulta.LinkedinUrl == organizationUpdateDTO.LinkedinUrl || string.IsNullOrEmpty(organizationUpdateDTO.LinkedinUrl))? consulta.LinkedinUrl : organizationUpdateDTO.LinkedinUrl;
           
            
            await _unitOfWork.OrganizationsRepository.Update(consulta);
            await _unitOfWork.SaveChangesAsync();


            return new Result().Success("Los cambios se han realizado correctamente");
        }
    }
}
