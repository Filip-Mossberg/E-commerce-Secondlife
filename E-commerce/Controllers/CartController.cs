using E_commerce.BLL.IService;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> ClearCart(int id)
        {
            var response = await _cartService.ClearCart(id);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
