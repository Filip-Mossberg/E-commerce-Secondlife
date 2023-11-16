using E_commerce.BLL.IService;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Category;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("Api/Category")]
    public class CategoryController : Controller
    {
        public readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory(CategoryCreateRequest categoryCreateReq)
        {
            var response = await _categoryService.CreateCategory(categoryCreateReq);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCategories()
        {
            var response = await _categoryService.GetAllCategories();
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var response = await _categoryService.GetCategoryById(id);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            var response = await _categoryService.UpdateCategory(category);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : NotFound();
        }

        [HttpDelete("DeleteById")]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            var response = await _categoryService.DeleteCategoryById(id);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : NotFound();
        }

    }
}
