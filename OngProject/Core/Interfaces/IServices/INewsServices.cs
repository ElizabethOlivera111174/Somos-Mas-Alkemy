using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Pagination;
using System;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface INewsServices
    {
        Task<NewsDTO> GetById(int id);

        bool EntityExists(int id);
        Task<Result> Insert(NewsInsertDTO newsInsertDTO);

        Task<Result> Delete(int id);
        Task<Result> Update(NewsUpdateDTO newsUpdateDTO, int id);
        Task<PaginationDTO<NewsDTO>> GetByPage(string route, int page);
    }
}
