using Azure.Storage.Blobs.Models;
using E_commerce.BLL.IService;
using E_commerce.Models.DTO_s.Image;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("Api/Image")]
    public class ImageController : Controller
    {
        private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllImagesById(int productId)
        {
            var response = await _imageService.GetAllImagesById2(productId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
