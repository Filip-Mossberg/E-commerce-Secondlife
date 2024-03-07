using E_commerce.Models;
using E_commerce.Models.DbModels;

namespace E_commerce.BLL.IService
{
    public interface ICartService
    {
        public Task CreateCart(User user);
        public Task<ApiResponse> ClearCart(string userId);
        public Task<ApiResponse> AddItemToCart(string userId, int productId);
        public Task<ApiResponse> RemoveItemFromCart(string userId, int productId);
        public Task<ApiResponse> GetCartItemsById(string userId);
    }
}
