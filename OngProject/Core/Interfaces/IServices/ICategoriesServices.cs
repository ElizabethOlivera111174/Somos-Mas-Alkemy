using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Pagination;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ICategoriesServices
    {

        Task<CategoryDTO> GetById(int id);
        bool EntityExist(int id);
        Task<IEnumerable<CategoryNameDTO>> GetAll();
        Task<PaginationDTO<CategoryDTO>> GetByPage(string route,int page);
        Task<Category> Post(CategoryDTO categoryDTO);
        Task<Result> Update(int id, UpdateCategoryDTO updateCategoryDTO);
        Task<Result> Delete(int id);
    }
}
