using Microsoft.AspNetCore.Http;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.SlidesDTOs;
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
    public class SlidesServices : ISlidesServices
    {
        #region Object and Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageServices;
        public SlidesServices(IUnitOfWork unitOfWork, IImageService imageServices)
        {
            _unitOfWork = unitOfWork;
            _imageServices = imageServices;
        } 
        #endregion


        public bool EntityExist(int id)
        {
            return _unitOfWork.SlidesRepository.EntityExists(id);
        }
        public async Task<IEnumerable<SlidesDTO>> GetAll()
        {
            var mapper = new EntityMapper();
            var slideList = await _unitOfWork.SlidesRepository.GetAll();
            var slideDTOList = slideList.Select(x => mapper.FromSlideToSlideDto(x)).ToList();
            return slideDTOList;
        }
        public async Task<List<SlidesPublicDTO>> GetAllPublic()
        {
            var mapper = new EntityMapper();
            var slideList = await _unitOfWork.SlidesRepository.GetAll();
            var orderSlideList = slideList.OrderBy(s => s.Order).ToList();
            var slideDTOList = mapper.FromSlidePublicToSlideDto(orderSlideList);
            return slideDTOList;
        }
        public async Task<SlidesDTO> GetById(int id)
        {
            var mapper = new EntityMapper();
            var slide = await _unitOfWork.SlidesRepository.GetById(id);
            var slideDTO = mapper.FromSlideToSlideDto(slide);
            return slideDTO;
        }
        public async Task<Result> Insert(SlidesCreateDTO slidesCreateDTO)
        {
            Slides slide;
            byte[] bytesFile = Convert.FromBase64String(slidesCreateDTO.ImageUrl);
            slidesCreateDTO.FileName = ValidateFiles.GetImageExtensionFromFile(bytesFile);
            string uniqueName = "slide_" + DateTime.Now.ToString().Replace(",", "").Replace("/", "").Replace(" ", "");
            var formFile = new FormFileData()
            {
                FileName = slidesCreateDTO.FileName,
                ContentType = slidesCreateDTO.ContentType,
                Name = slidesCreateDTO.Name
            };
            var image = ConvertFile.BinaryToFormFile(bytesFile,formFile);

            var urlImage = await _imageServices.Save(uniqueName + slidesCreateDTO.FileName, image);

            if(slidesCreateDTO.Order != 0)
            {
                slide = new Slides()
                {
                    ImageUrl = urlImage,
                    Text = slidesCreateDTO.Text,
                    Order = slidesCreateDTO.Order,
                    OrganizationId = slidesCreateDTO.OrganizationId
                };
            }
            else
            {
                var mapper = new EntityMapper();
                var slideList = await _unitOfWork.SlidesRepository.GetAll();
                var slideLast = slideList.OrderByDescending(s => s.Id).FirstOrDefault();
                slide = new Slides()
                {
                    ImageUrl = urlImage,
                    Text = slidesCreateDTO.Text,
                    Order = slideLast.Order,
                    OrganizationId = slidesCreateDTO.OrganizationId
                };
            }

            var response = await _unitOfWork.SlidesRepository.Insert(slide);
            await _unitOfWork.SaveChangesAsync();

            if (response != null)
            {
                return new Result().Success("Slide ingresado con éxito");
            }
            else
            {
                return new Result().Fail("No se ha podido ingresar el Slide");
            }
        }
        public async Task<Result> Update(int id, UpdateSlideDTO updateSlideDTO)
        {
            
            var slide = await _unitOfWork.SlidesRepository.GetById(id);
            if (slide == null)
                return new Result().NotFound();

            if (updateSlideDTO.ImageUrl!=null)
                slide.ImageUrl = await _imageServices.Save(slide.ImageUrl, updateSlideDTO.ImageUrl);

            slide.Order = updateSlideDTO.Order;
            slide.OrganizationId = updateSlideDTO.OrganizationId;
            slide.Text = updateSlideDTO.Text;
            
            await _unitOfWork.SlidesRepository.Update(slide);

            _unitOfWork.SaveChanges();

            return new Result().Success($"El item se ha modificado correctamente!! \n" +
                $" {slide.Text}");
        }
        public async Task<Result> Delete(int id)
        {
            var verify = _unitOfWork.SlidesRepository.GetById(id);
            if (verify==null)
            {
                return new Result().NotFound();
            }

            await _unitOfWork.SlidesRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();

            return new Result().Success($"Se ha borrado el slide correctamente");
        }

    }
}
