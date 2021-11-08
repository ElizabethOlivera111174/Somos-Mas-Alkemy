using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface IActivitiesServices
    {
        Task<IEnumerable<ActivitiesDTO>> GetAll();
        Task<ActivitiesDTO> GetById(int id);
        bool EntityExists(int id);
        Task<Result> Insert(ActivitiyInsertDTO activitiyInsertDTO);
        Task<Result> Update(ActivityUpdateDTO activityUpdateDTO,int id);
    }
}
