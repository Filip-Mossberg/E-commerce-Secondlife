using E_commerce.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.DAL.IRepository
{
    public interface ICartRepository
    {
        public Task CreateCart(Cart cart);
        public Task<bool> RemoveItemFromCart(string userId, Product product);
        public Task<bool> RemoveAllFromCart(string userId);
        public Task<Cart> GetCartById(string userId);
        public Task<bool> AddItemToCart(Product product, Cart cart);
        public Task<List<Product>> GetCartItems(string userId);
    }
}
