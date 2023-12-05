using E_commerce.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.DAL.IRepository
{
    public interface IOrderRepository
    {
        public Task<Order> PlaceOrder(Order order);
        public Task<IEnumerable<Order>> GetAllByUserId(string userId);
        public Task<IEnumerable<Order>> GetAllOrders();
        public Task CancelOrderById(Order order);
        public Task<Order> GetOrderById(int id);
    }
}
