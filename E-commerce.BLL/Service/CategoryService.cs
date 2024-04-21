using AutoMapper;
using E_commerce.BLL.IService;
using E_commerce.DAL.IRepository;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Category;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_commerce.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<CategoryCreateRequest> _categoryValidator;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        public CategoryService(ICategoryRepository categoryRepository, IValidator<CategoryCreateRequest> createCategoryValidator,
            IMapper mapper, IDistributedCache cahce)
        {
            _categoryRepository = categoryRepository;
            _categoryValidator = createCategoryValidator;
            _mapper = mapper;
            _cache = cahce;
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
                await _cache.RemoveAsync("category-all");

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

        public async Task<ApiResponse> GetAllCategoriesRedis()
        {
            ApiResponse response = new ApiResponse() { StatusCode = StatusCodes.Status400BadRequest };

            string key = "category-all";

            string? cachedCategories = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(cachedCategories))
            {
                var categories = await _categoryRepository.GetAllCategories();

                if (categories.Any())
                {
                    await _cache.SetStringAsync(key, JsonSerializer.Serialize(categories));

                    response.IsSuccess = true;
                    response.StatusCode = 200;
                    response.Result = categories;

                    return response;
                }

                return response;
            }

            response.IsSuccess = true;
            response.StatusCode = 200;
            response.Result = JsonSerializer.Deserialize<List<Category>>(cachedCategories);

            return response;
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

        public async Task<ApiResponse> GetCategoryByIdRedis(int id)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status404NotFound };

            string key = $"category-{id}";

            string? category = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(category))
            {
                var test = await _categoryRepository.GetCategoryById(id);

                if(test != null)
                {
                    await _cache.SetStringAsync(
                        key, 
                        JsonSerializer.Serialize(test));

                    response.IsSuccess = true;
                    response.StatusCode = 200;
                    response.Result = test;

                    return response;
                }

                return response;
            }

            response.IsSuccess = true;
            response.StatusCode = 200;
            response.Result = JsonSerializer.Deserialize<Category>(category);

            return response;
        }

        public async Task<ApiResponse> UpdateCategory(Category category)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var validationResult = await _categoryValidator.ValidateAsync(_mapper.Map<CategoryCreateRequest>(category));

            if(validationResult.IsValid)
            {
                await _categoryRepository.UpdateCategory(category);
                await _cache.RemoveAsync("category-all");
                await _cache.RemoveAsync($"category-{category.Id}");

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
                await _cache.RemoveAsync("category-all");
                await _cache.RemoveAsync($"category-{id}");

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
