using E_commerce.BLL.IService;
using E_commerce.BLL.Service;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Order;
using E_commerce.Models.DTO_s.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Globalization;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        //[Authorize(Roles = "User")]
        [HttpPost("create")]
        public async Task<IActionResult> PlaceOrder(PlaceOrderRequest placeOrderReq)
        {
            var response = await _orderService.PlaceOrder(placeOrderReq);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        //[Authorize(Roles = "Admin, User")]
        [HttpGet("getall/user")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var response = await _orderService.GetAllByUserId(userId);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await _orderService.GetAllOrders();
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        //[Authorize(Roles = "Admin, User")]
        [HttpDelete("cancel")]
        public async Task<IActionResult> CancelOrderById(int id)
        {
            var response = await _orderService.CancelOrderById(id);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
