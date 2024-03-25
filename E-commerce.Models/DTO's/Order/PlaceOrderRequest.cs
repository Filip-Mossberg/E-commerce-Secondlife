using E_commerce.Models.DTO_s.Product;

namespace E_commerce.Models.DTO_s.Order
{
    public class PlaceOrderRequest
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTime DateOrdered { get; set; } = DateTime.Now;
        public List<ProductOrderRequest> Products { get; set; }
    }
}
