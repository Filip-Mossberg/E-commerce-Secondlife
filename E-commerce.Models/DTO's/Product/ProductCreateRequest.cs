using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Image;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DTO_s.Product
{
    public class ProductCreateRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime DateListed { get; set; } = DateTime.Now;
        public List<ImageUploadRequest> Images { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
