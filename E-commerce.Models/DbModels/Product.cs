using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public bool IsOrdered { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
        public List<Cart> Carts { get; set; } = new List<Cart>();
    }
}
