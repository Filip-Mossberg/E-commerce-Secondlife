using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DbModels
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime DateListed { get; set; }
        public int UserId { get; set; }
        public User user { get; set; }
        public int CategoryId { get; set; }
        public Category category { get; set; }
        List<Image> images { get; set; } = new List<Image>();
        List<ProductCart> productCarts { get; set; } = new List<ProductCart>();
    }
}
