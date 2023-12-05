using E_commerce.Models;
using E_commerce.Models.DTO_s.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.IService
{
    public interface IOrderService
    {
        public Task<ApiResponse> GetAllOrders();
        public Task<ApiResponse> GetAllByUserId(string userId);
        public Task<ApiResponse> PlaceOrder(PlaceOrderRequest placeOrderReq);
        public Task<ApiResponse> CancelOrderById(int id);
    }
}
