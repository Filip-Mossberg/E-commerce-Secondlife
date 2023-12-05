using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DTO_s.Product
{
    public class ProductOrderRequest
    {
        public int Id { get; set; }
        public bool IsOrdered { get; set; } 
    }
}
