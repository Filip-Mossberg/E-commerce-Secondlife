using E_commerce.DAL.IRepository;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Category;
using E_commerce.Models;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using MediatR;

namespace E_commerce.BLL.Service.ServiceTest
{
    public class CreateCategoryService : IRequest<ApiResponse>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<CategoryCreateRequest> _categoryValidator;
        private readonly IMapper _mapper;
        public CreateCategoryService(ICategoryRepository categoryRepository, IValidator<CategoryCreateRequest> createCategoryValidator,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _categoryValidator = createCategoryValidator;
            _mapper = mapper;
        }

        public async Task<ApiResponse> Handle(CategoryCreateRequest request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            var validationResult = await _categoryValidator.ValidateAsync(request);
            var categoryExists = await _categoryRepository.GetCategoryByName(request.Name);

            if (validationResult.IsValid
                && categoryExists == null)
            {
                await _categoryRepository.CreateCategory(_mapper.Map<Category>(request));

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
                    response.Errors.Add($"Category with name {request.Name} already exists!");
                }

                return response;
            }
        }
    }
}
