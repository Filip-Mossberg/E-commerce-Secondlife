using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Product;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DTO_s.Order
{
    public class PlaceOrderRequest
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTime DateOrdered { get; set; } = DateTime.Now;
        public List<ProductOrderRequest> Products { get; set; }
    }
}
