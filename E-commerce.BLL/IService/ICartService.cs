using E_commerce.Models;
using E_commerce.Models.DbModels;

namespace E_commerce.BLL.IService
{
    public interface ICartService
    {
        public Task CreateCart(User user);
        public Task<ApiResponse> ClearCart(int id);
    }
}
