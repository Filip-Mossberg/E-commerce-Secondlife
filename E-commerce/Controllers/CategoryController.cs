using AutoMapper;
using E_commerce.BLL.IService;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Category;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IValidator<CategoryCreateRequest> _validator;
        private readonly IMapper _mapper;
        
        public CategoryController(ICategoryService categoryService, IPublishEndpoint publishEndpoint,
            IValidator<CategoryCreateRequest> validator, IMapper mapper)
        {
            _categoryService = categoryService;
            _publishEndpoint = publishEndpoint;
            _validator = validator;
            _mapper = mapper; 
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory(CategoryCreateRequest categoryCreateReq)
        {
            var response = await _categoryService.CreateCategory(categoryCreateReq);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCategories()
        {
            var response = await _categoryService.GetAllCategories();
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [AllowAnonymous]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var response = await _categoryService.GetCategoryById(id);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            var response = await _categoryService.UpdateCategory(category);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : NotFound();
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("DeleteById")]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            var response = await _categoryService.DeleteCategoryById(id);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : NotFound();
        }

        [AllowAnonymous]
        [HttpGet("GetByIdRedis")]
        public async Task<IActionResult> GetCategoryByIdRedis(int id)
        {
            var response = await _categoryService.GetCategoryByIdRedis(id);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [AllowAnonymous]
        [HttpGet("GetAllRedis")]
        public async Task<IActionResult> GetAllCategoriesRedis()
        {
            var response = await _categoryService.GetAllCategoriesRedis();
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [AllowAnonymous]
        [HttpPost("CreateMassTransit")]
        public async Task<IActionResult> CreateCategoryMassTransit([FromBody] CategoryCreateRequest categoryCreateReq)
        {
            var validationResult = await _validator.ValidateAsync(categoryCreateReq);

            if (validationResult.IsValid)
            {
                await _publishEndpoint.Publish<CategoryCreateRequest>(categoryCreateReq);

                return Ok();
            }

            return BadRequest(validationResult.Errors);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("UpdateMassTransit")]
        public async Task<IActionResult> UpdateCategoryMassTransit([FromBody] Category category)
        {
            var validationResult = await _validator.ValidateAsync(_mapper.Map<CategoryCreateRequest>(category));

            if (validationResult.IsValid)
            {
                await _publishEndpoint.Publish(category);

                return Ok();
            }

            return BadRequest(validationResult.Errors);
        }

    }
}
