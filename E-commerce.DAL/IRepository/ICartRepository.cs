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
        public Task<bool> RemoveItemFromCart(int cartId, Product product);
        public Task<bool> RemoveAllFromCart(int cartId);
        public Task<Cart> GetCartById(int id);
        public Task<bool> AddItemToCart(Product product, Cart cart);
    }
}
