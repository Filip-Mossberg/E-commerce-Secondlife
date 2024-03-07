using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DTO_s.Product
{
    public class ProductGetRequest
    {
        public string? searchTerm { get; set; }
        public string? sortColumn { get; set; }
        public string? sortOrder { get; set; }
        public int? category { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}