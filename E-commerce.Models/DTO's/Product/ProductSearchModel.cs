using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models
{
    public class ProductSearchModel
    {
        public string SearchTerm { get; set; }
        public int? CategoryId { get; set; } 
        public int MinAmount { get; set; }
        public int MaxAmount { get; set; }
    }
}
