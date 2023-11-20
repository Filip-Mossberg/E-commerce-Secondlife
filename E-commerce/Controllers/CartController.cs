using E_commerce.BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("Api/Cart")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpDelete("ClearCart")]
        public async Task<IActionResult> ClearCart(int cartId)
        {
            var response = await _cartService.ClearCart(cartId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(int cartId, int productId)
        {
            var response = await _cartService.AddItemToCart(cartId, productId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("RemoveSingle")]
        public async Task<IActionResult> RemoveItemFromCart(int cartId, int productId)
        {
            var response = await _cartService.RemoveItemFromCart(cartId, productId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
