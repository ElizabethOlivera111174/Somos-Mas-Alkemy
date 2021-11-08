using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Helper.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ICommentsServices
    {
        Task<IEnumerable<CommentsDTO>> GetAll();
        Task<PaginationDTO<CommentsDTO>> GetByPage(string route,int page);
        Task<IEnumerable<CommentsDTO>> GetById(int id);
        bool EntityExists(int id);
        Task<NewCommentsDTO> Insert(NewCommentsDTO newCommentsDTO);
        Task<Result> Delete(int id);
        Task<bool> ValidateCreatorOrAdmin(ClaimsPrincipal user, int id);
        Task<Result> Update(int id, CommentUpdateDTO CommentUpdateDTO);
    }
}
