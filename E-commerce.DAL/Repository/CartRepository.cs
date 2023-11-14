using E_commerce.Context;
using E_commerce.DAL.IRepository;
using E_commerce.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.DAL.Repository
{
    internal class CartRepository : ICartRepository
    {
        public readonly AppDbContext _context;
        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateCart(Cart cart)
        {
            await _context.Cart.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public Task GetCartById(string id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllFromCart()
        {
            throw new NotImplementedException();
        }

        public Task RemoveItemFromCart()
        {
            throw new NotImplementedException();
        }

        public Task RemoveItemFromTask()
        {
            throw new NotImplementedException();
        }
    }
}
