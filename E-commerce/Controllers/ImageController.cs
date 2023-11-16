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

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadImage(ImageUploadRequest imageUploadRequest)
        {
            var response = await _imageService.UploadImage(imageUploadRequest);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
