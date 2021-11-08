using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Helper.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ITestimonialsServices
    {
        Task<Result> Insert(TestimonialsCreateDTO testimonialsDTO);
        Task<Result> Delete(int id);
        Task<Result> Update(int id, TestimonialsUpdateDTO testimonialsUpdateDTO); 
        Task<PaginationDTO<TestimonialsDTO>> GetByPage(string route, int page);
    }
}
