using E_commerce.BLL.IService;
using E_commerce.Models.DTO_s.Email;
using E_commerce.Models.DTO_s.Order;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IValidator<PlaceOrderRequest> _validator;
        public OrderController(IOrderService orderService, IPublishEndpoint publishEndpoint, IValidator<PlaceOrderRequest> validator)
        {
            _orderService = orderService;
            _publishEndpoint = publishEndpoint;
            _validator = validator;
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

        //[Authorize(Roles = "User")]
        [HttpPost("create/masstransit")]
        public async Task<IActionResult> CreateOrderMassTransit([FromBody] PlaceOrderRequest placeOrderRequest)
        {
            var validatioResult = await _validator.ValidateAsync(placeOrderRequest);

            if (validatioResult.IsValid)
            {
                await _publishEndpoint.Publish(placeOrderRequest);

                var EmailMessageDTO = new EmailMessageDTO()
                {
                    To = new List<string> { placeOrderRequest.Email },
                    Subject = "Order Receipt",
                    Content = "You have now placed an order!"
                };

                await _publishEndpoint.Publish(EmailMessageDTO);

                return Ok();
            }

            return BadRequest(validatioResult.Errors);
        }
    }
}
