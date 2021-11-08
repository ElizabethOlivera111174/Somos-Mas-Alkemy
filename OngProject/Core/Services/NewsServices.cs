using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Entities;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Mapper;
using OngProject.Infrastructure.Repositories.IRepository;
using System.Threading.Tasks;
using OngProject.Core.Helper.Pagination;
using System;
using System.Linq;

namespace OngProject.Core.Services
{
    public class NewsServices : INewsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUriService _uriService;
        private readonly IImageService _imageServices;
        private readonly EntityMapper _entityMapper;
        public NewsServices(IUnitOfWork unitOfWork, IImageService imageServices, IUriService uriService)
        {
            _unitOfWork = unitOfWork;
            _imageServices = imageServices;
            _uriService = uriService;
            _entityMapper = new EntityMapper();
        }

        public async Task<NewsDTO> GetById(int id)
        {
            var news = await _unitOfWork.NewsRepository.GetById(id);
            var newsDTO = _entityMapper.FromNewsToNewsDTO(news);
           
            return newsDTO;

        }

        public async Task<PaginationDTO<NewsDTO>> GetByPage(string route, int page)
        {
            if (page <= 0) page = 1;
            const int elementsByPage = 10;
            var n = await _unitOfWork.NewsRepository.GetPageAsync(x => x.Name, elementsByPage, page);
            var items = n.ToList();
            var itemsList = items.Select(x => _entityMapper.FromNewsToNewsDTO(x)).ToList();
            var totalItems = await _unitOfWork.NewsRepository.CountAsync();
            var totalpages = (int)Math.Ceiling((double)totalItems / elementsByPage);

            if (page > totalpages)
            {
                throw new Exception();
            }

            var response = new PaginationDTO<NewsDTO>()
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

        public bool EntityExists(int id)
        {
            return _unitOfWork.NewsRepository.EntityExists(id);
        }

        public async Task<Result> Insert(NewsInsertDTO newsInsertDTO)
        {
            var newNews = _entityMapper.NewsInsertDTOtoNews(newsInsertDTO);

            string uniqueName = "News_" + DateTime.Now.ToString().Replace(",", "").Replace("/", "").Replace(" ", "");
            var urlImage = await _imageServices.Save(uniqueName + newsInsertDTO.Image.FileName, newsInsertDTO.Image);
            newNews.Image = urlImage;

            await _unitOfWork.NewsRepository.Insert(newNews);
            await _unitOfWork.SaveChangesAsync();

            return new Result().Success("Se ha insertado correctamente la novedad");

        }

        public async Task<Result> Delete(int id)
        {
            var news = await _unitOfWork.NewsRepository.GetById(id);
            if (news == null)
            {
                return new Result().NotFound();
            }
            var url = news.Image;
            var result = await _unitOfWork.NewsRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            if (result != null)
            {
                try
                {
                    await _imageServices.Delete(url);
                }
                catch(Exception)
                {

                }

                return new Result().Success("Novedad eliminada con exito");
            }
            return new Result().Fail("Ocurrio un error al eliminar la novedad");
        }

        public async Task<Result> Update(NewsUpdateDTO newsUpdateDTO,int id)
        {
            var news = await _unitOfWork.NewsRepository.GetById(id);
            if (news == null) return new Result().NotFound();

            if (!string.IsNullOrEmpty(newsUpdateDTO.Image.Name))
            {
                
                try
                {
                    await _imageServices.Delete(news.Image);
                }
                catch(Exception)
                {

                }
                string uniqueName = "News_" + DateTime.Now.ToString().Replace(",", "").Replace("/", "").Replace(" ", "");
                news.Image = await _imageServices.Save(uniqueName + newsUpdateDTO.Image.FileName, newsUpdateDTO.Image);
            }

            if(!string.IsNullOrEmpty(newsUpdateDTO.Content)) news.Content = newsUpdateDTO.Content;
            if(!string.IsNullOrEmpty(newsUpdateDTO.Name)) news.Name = newsUpdateDTO.Name;
            if(newsUpdateDTO.CategoryId > 0)
            {
                if(_unitOfWork.CategoryRepository.GetById(newsUpdateDTO.CategoryId) != null)
                {
                    news.CategoryId = newsUpdateDTO.CategoryId;
                }
                
            }

            await _unitOfWork.NewsRepository.Update(news);
            _unitOfWork.SaveChanges();

            return new Result().Success($"La Novedad se modifico correctamente!!");
        }
    }
}
