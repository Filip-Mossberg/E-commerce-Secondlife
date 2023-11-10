using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DbModels
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime DateOrdered { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();

    }
}
