using Microsoft.Extensions.Configuration;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Mapper;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class ContactsServices : IContactsServices
    {
        #region Objects and Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly IUriService uriService;
        public ContactsServices(IUnitOfWork unitOfWork, IMailService mailService, IUriService uriService)
        {
            this._unitOfWork = unitOfWork;
            this._mailService = mailService;
            this.uriService = uriService;
        }
        #endregion
        public async Task<PaginationDTO<ContactDTO>> GetByPage(string route, int page)
        {
            if (page <= 0) page = 1;
            int elementsByPage = 10; // Condición exigida por negocio
            var m = await _unitOfWork.ContactsRepository.GetPageAsync(x => x.Name, elementsByPage, page);
            var items = m.ToList();
            var mapper = new EntityMapper();
            var itemsList = items.Select(x => mapper.FromContactsToContactsDto(x)).ToList();
            var totalItems = await _unitOfWork.ContactsRepository.CountAsync();
            var totalpages = (int)Math.Ceiling((double)totalItems / elementsByPage);

            if (page > totalpages)
            {
                return null;
            }

            var response = new PaginationDTO<ContactDTO>()
            {
                CurrentPage = page,
                TotalItems = totalItems,
                TotalPages = totalpages,
                PrevPage = page==totalpages || page > totalpages ? uriService.GetPage(route, page - 1) : null,
                NextPage = page < totalpages ? uriService.GetPage(route, page + 1) : null,
                Items = itemsList
            };

            return response;
        }
        public async Task<ContactDTO> GetById(int id)
        {

            var mapper = new EntityMapper();
            var contact = await _unitOfWork.ContactsRepository.GetById(id);
            var contactDTO = mapper.FromContactsToContactsDto(contact);
            return contactDTO;
        }
        public bool EntityExists(int id)
        {
            return _unitOfWork.ContactsRepository.EntityExists(id);
        }
        public async Task<Result> Insert(ContactInsertDTO contactInsertDTO)
        {
            var mapper = new EntityMapper();

            var newContact = mapper.FromContactsDtoToContacts(contactInsertDTO);
            await _unitOfWork.ContactsRepository.Insert(newContact);

            await _unitOfWork.SaveChangesAsync();

            string body = $"{newContact.Name}, Gracias por el contacto! En breve nos estaremos comunicando con vos";
            await _mailService.SendEmailAsync(newContact.Email, body, $"Contacto OnG para {newContact.Name}");

            return new Result().Success("El contacto fue creado correctamente");
        }
    }
}
