using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DTO_s.Product
{
    public class ProductGetResponse
    {
        public ProductGetResponse()
        {
            
        }
        public ProductGetResponse(
            int id,
            string title,
            string description,
            string category,
            int price,
            DateTimeOffset dateListed,
            List<ImageGetRequest> images)
        {
            Id = id;
            Title = title;
            Description = description;
            Category = category;
            Price = price;
            DateListed = dateListed;
            Images = images ?? new List<ImageGetRequest>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public DateTimeOffset DateListed { get; set; }
        public List<ImageGetRequest> Images { get; set; } = new List<ImageGetRequest>();
    }
}
