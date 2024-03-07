using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DTO_s.Order
{
    public class OrderGetRequest
    {
        public int Id { get; set; }
        public DateTime DateOrdered { get; set; }
        public string UserId { get; set; }
        public List<ProductGetResponse> Products { get; set; } = new List<ProductGetResponse>();
    }
}
