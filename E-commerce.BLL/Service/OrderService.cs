using AutoMapper;
using E_commerce.BLL.IService;
using E_commerce.DAL.IRepository;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Order;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Azure;
using Serilog;
using static System.Net.Mime.MediaTypeNames;

namespace E_commerce.BLL.Service
{
    public class OrderService : IOrderService
    {
        private readonly IValidator<PlaceOrderRequest> _validator;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IEmailService _emailService;
        public OrderService(IValidator<PlaceOrderRequest> validator, IOrderRepository orderRepository,
            IMapper mapper, IProductRepository productRepository, IEmailService emailServices)
        {
            _validator = validator;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _emailService = emailServices;

        }

        public async Task<ApiResponse> CancelOrderById(int id)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            var orderToCancel = await _orderRepository.GetOrderById(id);

            if(orderToCancel != null)
            {
                foreach (var product in orderToCancel.Products.ToList())
                {
                    product.IsOrdered = false;
                    product.OrderId = null;

                    await _productRepository.UpdateProduct(product);
                }
                await _orderRepository.CancelOrderById(orderToCancel);

                response.IsSuccess = true;
                response.StatusCode = StatusCodes.Status200OK;
                return response;
            }
            else
            {
                response.Errors.Add($"Unable to remove order with id {id}");
                return response;
            }
        }

        public async Task<ApiResponse> GetAllByUserId(string userId)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            var orders = await _orderRepository.GetAllByUserId(userId);
            var userOrders = _mapper.Map<IEnumerable<OrderGetRequest>>(orders);

            if (orders.Any())
            {
                response.IsSuccess = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Result = userOrders;
                return response;
            }
            else
            {
                response.Errors.Add("You dont have any orders!");
                return response;
            }
        }

        public async Task<ApiResponse> GetAllOrders()
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            var orders = await _orderRepository.GetAllOrders();
            var mappedOrders = _mapper.Map<IEnumerable<OrderGetRequest>>(orders);

            if (orders.Any())
            {
                response.IsSuccess = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Result = mappedOrders;
                return response;
            }
            else
            {
                response.Errors.Add("Unable to retrieve orders!");
                return response;
            }
        }

        public async Task<ApiResponse> PlaceOrder(PlaceOrderRequest placeOrderReq)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var validationResult = await _validator.ValidateAsync(placeOrderReq);
            List<Product> products = new List<Product>();

            if(validationResult.IsValid)
            {
                if(await _productRepository.UserProducts(placeOrderReq.UserId) <= 20)
                {
                    if (!placeOrderReq.Products.Exists(p => p.IsOrdered == true))
                    {
                        foreach (var product in placeOrderReq.Products)
                        {
                            products.Add(await _productRepository.GetProductById(product.Id));
                        }
                        var placedOrder = await _orderRepository.PlaceOrder(_mapper.Map<Order>(placeOrderReq));
                        placedOrder.Products = products;

                        foreach (var product in placedOrder.Products)
                        {
                            product.IsOrdered = true;
                            product.OrderId = placedOrder.Id;

                            await _productRepository.UpdateProduct(product);
                        }

                        var message = new EmailMessage(new string[] { placeOrderReq.Email }, 
                            $"Your Order Confirmation - Order #{placedOrder.Id}", 
                            "<html><body><h2>Thank you for your order!</h2><h3>Order Details:</h3><p>Order Number: #" + placedOrder.Id + "<br>Order Date: " + placedOrder.DateOrdered.ToString("yyyy-MM-dd") + "<br>Order Items: <br><ul><li>IPhone Xs - $1299.99</li></ul></p></body></html>");
                        _emailService.SendEmail(message);

                        response.IsSuccess = true;
                        response.StatusCode = StatusCodes.Status201Created;
                        return response;
                    }
                    else
                    {
                        response.Errors.Add("Remove gray items from cart!");
                        return response;
                    }
                }
                else
                {
                    response.Errors.Add("You have the maximal amount of products for sale (20)");
                    return response;
                }
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    response.Errors.Add(error.ErrorMessage);
                }

                return response;
            }
        }
    }
}
