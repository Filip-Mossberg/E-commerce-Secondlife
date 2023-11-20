using AutoMapper;
using E_commerce.BLL.IService;
using E_commerce.DAL.IRepository;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Category;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<CategoryCreateRequest> _categoryValidator;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IValidator<CategoryCreateRequest> createCategoryValidator,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _categoryValidator = createCategoryValidator;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreateCategory(CategoryCreateRequest createCategoryReq)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            var validationResult = await _categoryValidator.ValidateAsync(createCategoryReq);
            var categoryExists = await _categoryRepository.GetCategoryByName(createCategoryReq.Name);

            if (validationResult.IsValid
                && categoryExists == null)
            {
                await _categoryRepository.CreateCategory(_mapper.Map<Category>(createCategoryReq));

                response.StatusCode = StatusCodes.Status201Created;
                response.IsSuccess = true;
                return response;
            }
            else
            {
                if (validationResult.Errors.Any())
                {
                    foreach (var error in validationResult.Errors)
                    {
                        response.Errors.Add(error.ErrorMessage);
                    }
                }
                else
                {
                    response.Errors.Add($"Category with name {createCategoryReq.Name} already exists!");
                }

                return response;
            }
        }

        public async Task<ApiResponse> GetAllCategories()
        {
            ApiResponse response = new ApiResponse() { StatusCode = StatusCodes.Status400BadRequest };
            var categories = await _categoryRepository.GetAllCategories();

            if (categories.Any())
            {
                response.IsSuccess = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Result = categories;
                return response;
            }
            else
            {
                response.Errors.Add("Unable to get categories.");
                return response;
            }
        }

        public async Task<ApiResponse> GetCategoryById(int id)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status404NotFound };
            var category = await _categoryRepository.GetCategoryById(id);

            if(category != null)
            {
                response.IsSuccess = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Result = category;
                return response;
            }
            else
            {
                response.Errors.Add($"Could not get user with Id {id}");
                return response;
            }
        }

        public async Task<ApiResponse> UpdateCategory(Category category)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var validationResult = await _categoryValidator.ValidateAsync(_mapper.Map<CategoryCreateRequest>(category));

            if(validationResult.IsValid)
            {
                await _categoryRepository.UpdateCategory(category);

                response.IsSuccess = true;
                response.StatusCode = StatusCodes.Status200OK;
                return response;
            }
            else
            {
                if (validationResult.Errors.Any())
                {
                    foreach (var error in validationResult.Errors)
                    {
                        response.Errors.Add(error.ErrorMessage);
                    }
                }

                return response;
            }
        }

        public async Task<ApiResponse> DeleteCategoryById(int id)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var categoryExists = await _categoryRepository.GetCategoryById(id);

            if(categoryExists != null)
            {
                await _categoryRepository.DeleteCategory(categoryExists);

                response.StatusCode = StatusCodes.Status200OK;
                response.IsSuccess = true;
                return response;
            }
            else
            {
                response.Errors.Add($"Unable to delete category with id {id}");
                return response;
            }
        }
    }
}
