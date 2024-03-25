using E_commerce.BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        //[Authorize(Roles = "Admin, User")]
        [HttpDelete("ClearCart")]
        public async Task<IActionResult> ClearCart([FromQuery] string userId)
        {
            var response = await _cartService.ClearCart(userId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        //[Authorize(Roles = "Admin, User")]
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart([FromQuery] string userId, [FromQuery] int productId)
        {
            var response = await _cartService.AddItemToCart(userId, productId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        //[Authorize(Roles = "Admin, User")]
        [HttpDelete("RemoveSingle")]
        public async Task<IActionResult> RemoveItemFromCart([FromQuery] string userId, [FromQuery] int productId)
        {
            var response = await _cartService.RemoveItemFromCart(userId, productId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        //[Authorize(Roles = "Admin, User")]
        [HttpGet("GetUserCartItems")]
        public async Task<IActionResult> GetAllCartITems([FromQuery] string userId)
        {
            var response = await _cartService.GetCartItemsById(userId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
