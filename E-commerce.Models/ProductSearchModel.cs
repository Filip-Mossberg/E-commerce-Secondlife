using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models
{
    public class ProductSearchModel
    {
        public string? productName { get; set; }
        public int? CategoryId { get; set; }
        public int? PriceMax { get; set; }
        public int? PriceMin { get; set; }
    }
}
