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
        public Task RemoveItemFromCart();
        public Task RemoveAllFromCart();
        public Task GetCartById(string id);
    }
}
