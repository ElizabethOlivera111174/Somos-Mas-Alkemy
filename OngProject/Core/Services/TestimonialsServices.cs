using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.SlidesDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Pagination;
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
    public class TestimonialsServices : ITestimonialsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUriService _uriService;
        private readonly IImageService _imageServices;
        private readonly EntityMapper _mapper;

        public TestimonialsServices(IUnitOfWork unitOfWork, IImageService imageServices, IUriService uriService)
        {
            _uriService = uriService;
            _unitOfWork = unitOfWork;
            _imageServices = imageServices;
            _mapper = new EntityMapper();
        }
        public async Task<Result> Insert(TestimonialsCreateDTO testimonialsDTO)
        {
            if(testimonialsDTO.Imagen != null)
            {
                var newTestimonial = _mapper.TestimonialsCreateDTOTestimonials(testimonialsDTO);
                string uniqueName = "Testimonials_" + DateTime.Now.ToString().Replace(",", "").Replace("/", "").Replace(" ", "");
                var urlImage = await _imageServices.Save(uniqueName + testimonialsDTO.Imagen.FileName, testimonialsDTO.Imagen);
                newTestimonial.Image = urlImage;

                await _unitOfWork.TestimonialsRepository.Insert(newTestimonial);
                await _unitOfWork.SaveChangesAsync();

                return new Result().Success("Se ha insertado correctamente la actividad");
            }
            else
            {
                var newTestimonial = _mapper.TestimonialsCreateDTOTestimonials(testimonialsDTO);
                newTestimonial.Image = "";
                await _unitOfWork.TestimonialsRepository.Insert(newTestimonial);
                await _unitOfWork.SaveChangesAsync();

                return new Result().Success("Se ha insertado correctamente la actividad");
            }
        }
        public async Task<Result> Delete(int id)
        {
            var testimonial = await _unitOfWork.TestimonialsRepository.GetById(id);
            if(testimonial == null)
            {
                return new Result().NotFound();
            }
            var ulr = testimonial.Image;
            var result = await _unitOfWork.TestimonialsRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            if(result != null)
            {
                await _imageServices.Delete(ulr);
                return new Result().Success("Testimonial eliminado con exito");
            }
            return new Result().Fail("Ocurrio un error al eliminar el testimonial");
        }
        public async Task<Result> Update(int id, TestimonialsUpdateDTO testimonialsUpdateDTO)
        {
            var testimonial = await _unitOfWork.TestimonialsRepository.GetById(id);
            if (testimonial == null)
                return new Result().NotFound();
            
            if(testimonialsUpdateDTO.Imagen != null && string.IsNullOrEmpty(testimonial.Image))
            {
                await _imageServices.Delete(testimonial.Image);
                string uniqueName = "Testimonials_" + DateTime.Now.ToString().Replace(",", "").Replace("/", "").Replace(" ", "");
                testimonial.Image = await _imageServices.Save(uniqueName + testimonialsUpdateDTO.Imagen.FileName, testimonialsUpdateDTO.Imagen);
            }
            else
            {
                if(testimonialsUpdateDTO.Imagen != null)
                {
                    string uniqueName = "Testimonials_" + DateTime.Now.ToString().Replace(",", "").Replace("/", "").Replace(" ", "");
                    testimonial.Image = await _imageServices.Save(uniqueName + testimonialsUpdateDTO.Imagen.FileName, testimonialsUpdateDTO.Imagen);
                }
            }

            testimonial.Name = testimonialsUpdateDTO.Name;
            testimonial.Content = testimonialsUpdateDTO.Content;

            await _unitOfWork.TestimonialsRepository.Update(testimonial);
            await _unitOfWork.SaveChangesAsync();

            return new Result().Success($"El item se ha modificado correctamente!!");
        }

        public async Task<PaginationDTO<TestimonialsDTO>> GetByPage(string route, int page)
        {
            if (page <= 0) page = 1;
            int elementsByPage = 10; ;
            var n = await _unitOfWork.TestimonialsRepository.GetPageAsync(x => x.Name, elementsByPage, page);
            var items = n.ToList();
            var mapper = new EntityMapper();
            var itemsList = items.Select(x => mapper.FromTestimonialsToTestimonialsDTO(x)).ToList();
            var totalItems = await _unitOfWork.TestimonialsRepository.CountAsync();
            var totalpages = (int)Math.Ceiling((double)totalItems / elementsByPage);

            if (page > totalpages)
            {
                return null;
            }

            var response = new PaginationDTO<TestimonialsDTO>()
            {
                CurrentPage = page,
                TotalItems = totalItems,
                TotalPages = totalpages,
                PrevPage = page == totalpages || page > totalpages ? _uriService.GetPage(route, page - 1) : null,
                NextPage = page < totalpages ? _uriService.GetPage(route, page + 1) : null,
                Items = itemsList
            };

            return response;
        }
    }
}
