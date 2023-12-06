using Azure;
using E_commerce.BLL.IService;
using E_commerce.Models;
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
        [HttpPost("Search")]
        public async Task<IActionResult> ProductSearch(ProductSearchModel model)
        {
            var response = await _productService.ProductSearch(model);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        //[Authorize(Roles = "Admin, User")]
        [HttpGet("GetAllByUserId")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var response = await _productService.GetAllByUserId(userId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
