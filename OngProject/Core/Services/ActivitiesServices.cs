using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.FomFileData;
using OngProject.Core.Helper.S3;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Mapper;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class ActivitiesServices : IActivitiesServices
    {

        #region Object and Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly EntityMapper _mapper;
        private readonly IImageService _imageServices;
        public ActivitiesServices(IUnitOfWork unitOfWork, IImageService imageServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = new EntityMapper();
            _imageServices = imageServices;
        }
        #endregion
        public async Task<IEnumerable<ActivitiesDTO>> GetAll()
        {
            var activitiesList = await _unitOfWork.ActivitiesRepository.GetAll();
            var activitiesDTO = activitiesList.Select(x => _mapper.FromActivitiesToActivitiesDTO(x)).ToList();

            return activitiesDTO;
        }
        public async Task<ActivitiesDTO> GetById(int id)
        {
            var activities = await _unitOfWork.ActivitiesRepository.GetById(id);
            var activitiesDTO = _mapper.FromActivitiesToActivitiesDTO(activities);

            return activitiesDTO;
        }
        public async Task<Result> Insert(ActivitiyInsertDTO activitiyInsertDTO)
        {
            var newActivity = _mapper.ActivitiyInsertDTOtoActivity(activitiyInsertDTO);

            string uniqueName = "Activity_" + DateTime.Now.ToString().Replace(",", "").Replace("/", "").Replace(" ", "");
            var urlImage = await _imageServices.Save(uniqueName + activitiyInsertDTO.Image.FileName, activitiyInsertDTO.Image);
            newActivity.Image = urlImage;

            await _unitOfWork.ActivitiesRepository.Insert(newActivity);
            await _unitOfWork.SaveChangesAsync();

            return new Result().Success("Se ha insertado correctamente la actividad");
        }
        public bool EntityExists(int id)
        {
            return _unitOfWork.ActivitiesRepository.EntityExists(id);
        }
        public async Task<Result> Update(ActivityUpdateDTO activityUpdateDTO, int id)
        {
            var activity = await _unitOfWork.ActivitiesRepository.GetById(id);
            if (activity == null) return new Result().NotFound();

            if (activityUpdateDTO.Image != null)
            {
                string uniqueName = "Activity_" + DateTime.Now.ToString().Replace(",", "").Replace("/", "").Replace(" ", "");
                await _imageServices.Delete(activity.Image);
                activity.Image = await _imageServices.Save(uniqueName + activityUpdateDTO.Image.FileName, activityUpdateDTO.Image);
            }

            if (activityUpdateDTO.Content != null) activity.Content = activityUpdateDTO.Content;
            if (activityUpdateDTO.Name != null) activity.Name = activityUpdateDTO.Name;

            await _unitOfWork.ActivitiesRepository.Update(activity);

            _unitOfWork.SaveChanges();

            return new Result().Success($"El item se ha modificado correctamente!!" +
                $" {activity.Name}");

        }
    }
}
