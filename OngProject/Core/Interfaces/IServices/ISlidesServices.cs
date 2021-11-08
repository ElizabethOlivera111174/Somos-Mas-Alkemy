using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.SlidesDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ISlidesServices
    {
        Task<Result> Insert(SlidesCreateDTO slidesCreateDTO);
        Task<Result> Delete(int id);
        Task<IEnumerable<SlidesDTO>> GetAll();
        Task<List<SlidesPublicDTO>> GetAllPublic();
        Task<SlidesDTO> GetById(int id);
        bool EntityExist(int id);
        Task<Result> Update(int id, UpdateSlideDTO updateSlideDTO);
    }
}
