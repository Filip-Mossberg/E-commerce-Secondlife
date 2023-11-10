using Microsoft.AspNetCore.Identity;

namespace E_commerce.Models.DbModels
{
    public class User : IdentityUser
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Cart cart { get; set; }
        public List<Product> products { get; set; } = new List<Product>();
    }
}
