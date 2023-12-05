using E_commerce.BLL.IService;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> ClearCart(int cartId)
        {
            var response = await _cartService.ClearCart(cartId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        //[Authorize(Roles = "Admin, User")]
        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(int cartId, int productId)
        {
            var response = await _cartService.AddItemToCart(cartId, productId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        //[Authorize(Roles = "Admin, User")]
        [HttpDelete("RemoveSingle")]
        public async Task<IActionResult> RemoveItemFromCart(int cartId, int productId)
        {
            var response = await _cartService.RemoveItemFromCart(cartId, productId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
