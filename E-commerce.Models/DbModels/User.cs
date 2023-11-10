using Microsoft.AspNetCore.Identity;

namespace E_commerce.Models.DbModels
{
    public class User : IdentityUser
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Cart Cart { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
