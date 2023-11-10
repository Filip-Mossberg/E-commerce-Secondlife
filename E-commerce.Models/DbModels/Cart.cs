using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DbModels
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User user { get; set; }
        public List<ProductCart> productCart { get; set; } = new List<ProductCart>();
    }
}
