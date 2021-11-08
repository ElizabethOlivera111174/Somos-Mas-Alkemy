using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Helper.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface IContactsServices
    {
        Task<Result> Insert(ContactInsertDTO contactInsertDTO);
        Task<PaginationDTO<ContactDTO>> GetByPage(string route, int page);
        Task<ContactDTO> GetById(int id);
        bool EntityExists(int id);
    }
}
