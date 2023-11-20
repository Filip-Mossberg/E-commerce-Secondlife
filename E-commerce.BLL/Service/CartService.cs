using E_commerce.BLL.IService;
using E_commerce.DAL.IRepository;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        // No controller for this, since its automated
        // We invoke this Endpoint into the user register Endpoint
        public async Task CreateCart(User user)
        {
            Cart cart = new Cart()
            {
                UserId = user.Id,
                User = user
            };

            await _cartRepository.CreateCart(cart);
        }

        public async Task<ApiResponse> ClearCart(int cartId)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            
            var result = await _cartRepository.RemoveAllFromCart(cartId);
            if(result)
            {
                response.IsSuccess = true;
                response.StatusCode = StatusCodes.Status200OK;
                return response;
            }
            else
            {
                response.Errors.Add("Unable to clear cart!");
                return response;    
            }
        }

        public async Task<ApiResponse> AddItemToCart(int cartId, int productId)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            
            var productToAdd = await _productRepository.GetProductById(productId);
            var cartToAdd = await _cartRepository.GetCartById(cartId);

            if(productToAdd != null && cartToAdd != null)
            {
                var result = await _cartRepository.AddItemToCart(productToAdd, cartToAdd);

                if (result)
                {
                    response.IsSuccess = true;
                    response.StatusCode = StatusCodes.Status200OK;
                    return response;
                }
                else
                {
                    response.Errors.Add("Item already in cart!");
                    return response;
                }
            }
            else
            {
                response.Errors.Add("Unable to add item to cart!");
                return response;
            }
        }

        public async Task<ApiResponse> RemoveItemFromCart(int cartId, int productId)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            var productToRemove = await _productRepository.GetProductById(productId);

            if(productToRemove != null)
            {
                var result = await _cartRepository.RemoveItemFromCart(cartId, productToRemove);

                if (result)
                {
                    response.IsSuccess = true;
                    response.StatusCode = StatusCodes.Status200OK;
                    return response;
                }
                else
                {
                    response.Errors.Add("Product does not excist in the cart!");
                    return response;
                }
            }
            else
            {
                response.Errors.Add("Unable to remove item from cart!");
                return response;
            }
        }
    }
}
