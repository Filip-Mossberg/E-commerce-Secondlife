using Azure;
using E_commerce.BLL.IService;
using E_commerce.Models.DTO_s.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        //[Authorize(Roles = "User")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct(ProductCreateRequest productCreateRequest)
        {
            var response = await _productService.CreateProduct(productCreateRequest);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        //[Authorize(Roles = "Admin, User")]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var response = await _productService.DeleteProductById(productId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        //[Authorize(Roles = "Admin, User")]
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProduct(ProductUpdateRequest productUpdateRequest)
        {
            var response = await _productService.UpdateProduct(productUpdateRequest);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [AllowAnonymous]
        [HttpGet("SearchByProductName")]
        public async Task<IActionResult> SearchByProductName(string productName) 
        {
            var response = await _productService.SearchByProductName(productName);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [AllowAnonymous]
        [HttpGet("SearchByCategoryId")]
        public async Task<IActionResult> SearchByCategoryId(int categoryId)
        {
            var response = await _productService.GetAllByCategoryId(categoryId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
