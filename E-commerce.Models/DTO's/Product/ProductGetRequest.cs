using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DTO_s.Product
{
    public class ProductGetRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsOrdered { get; set; }
        public DateTime DateListed { get; set; }
        public List<ImageGetRequest> Images { get; set; } = new List<ImageGetRequest>();
    }
}
