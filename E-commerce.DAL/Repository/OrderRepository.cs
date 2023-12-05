using E_commerce.Context;
using E_commerce.DAL.IRepository;
using E_commerce.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.DAL.Repository
{
    internal class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CancelOrderById(Order order)
        {
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllByUserId(string userId)
        {
            return await _context.Order.Where(o => o.UserId == userId)
                .Include(o => o.Products)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Order
                .Include(o => o.Products)
                .ToListAsync();
        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _context.Order
                .Include(o => o.Products)
                .FirstOrDefaultAsync(order => order.Id == id);
        }

        public async Task<Order> PlaceOrder(Order order)
        {
            var result = await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
