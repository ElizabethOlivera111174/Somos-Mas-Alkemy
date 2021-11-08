using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Pagination;
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
    public class CategoriesServices : ICategoriesServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageServices;
        private readonly IUriService _uriService;
        public CategoriesServices(IUnitOfWork unitOfWork, IImageService imageServices, IUriService uriService)
        {
            _unitOfWork = unitOfWork;
            _imageServices = imageServices;
            _uriService = uriService;
        }

        public bool EntityExist(int id)
        {
            return _unitOfWork.CategoryRepository.EntityExists(id);
        }

        public async Task<Result> Delete(int id)
        {
            if (!_unitOfWork.CategoryRepository.EntityExists(id)) return new Result().NotFound();

            await _unitOfWork.CategoryRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();

            return new Result().Success($"Se ha borrado la categoria correctamente");
        }

        public async Task<IEnumerable<CategoryNameDTO>> GetAll()
        {
            var mapper = new EntityMapper();
            var CategoryList = await _unitOfWork.CategoryRepository.GetAll();
            var CategoryDTOList = CategoryList.Select(x => mapper.FromCategoryToCategoryNameDto(x)).ToList();
            return CategoryDTOList;
        }
        

        public async Task<CategoryDTO> GetById(int id)
        {
            var mapper = new EntityMapper();
            var category = await _unitOfWork.CategoryRepository.GetById(id);
            var categoryDTO = mapper.FromCategoryToCategoryDto(category);
            return categoryDTO;
        }

        public async Task<Category> Post(CategoryDTO categoryDTO)
        {
            var mapper = new EntityMapper();
            var category = mapper.FromCategoryCreateDtoToCategory(categoryDTO);
            await _unitOfWork.CategoryRepository.Insert(category);
            await _unitOfWork.SaveChangesAsync();
            return category;

        }

        public async Task<Result> Update(int id, UpdateCategoryDTO updateCategoryDTO)
        {
            var category = await _unitOfWork.CategoryRepository.GetById(id);
            if (category == null) return new Result().NotFound();

            if (updateCategoryDTO.Image != null) category.Image = await _imageServices.Save(category.Image, updateCategoryDTO.Image);
            if (updateCategoryDTO.Description != null) category.Description = updateCategoryDTO.Description;
            if (updateCategoryDTO.Name != null) category.Name = updateCategoryDTO.Name;

            await _unitOfWork.CategoryRepository.Update(category);

            _unitOfWork.SaveChanges();

            return new Result().Success($"El item se ha modificado correctamente!!" +
                $" {category.Name}");
        }

        public async Task<PaginationDTO<CategoryDTO>> GetByPage(string route, int page)
        {
            if (page <= 0) page = 1;
            int elementsByPage = 10; // Condición exigida por negocio
            var m = await _unitOfWork.CategoryRepository.GetPageAsync(x=> x.Name, elementsByPage, page);
            var items= m.ToList();
            var mapper = new EntityMapper();
            var itemsList = items.Select(x => mapper.FromCategoryToCategoryDto(x)).ToList();
            var totalItems = await _unitOfWork.CategoryRepository.CountAsync();
            var totalpages = (int)Math.Ceiling((double)totalItems / elementsByPage);

            var response = new PaginationDTO<CategoryDTO>()
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
