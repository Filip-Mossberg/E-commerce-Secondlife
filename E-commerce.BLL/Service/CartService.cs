using E_commerce.BLL.IService;
using E_commerce.DAL.IRepository;
using E_commerce.Models;
using E_commerce.Models.DbModels;
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
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
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

        public async Task<ApiResponse> ClearCart(int id)
        {
            ApiResponse response = new ApiResponse();
            
            await _cartRepository.RemoveAllFromCart(id);

            response.IsSuccess = true;
            return response;
        }
    }
}
