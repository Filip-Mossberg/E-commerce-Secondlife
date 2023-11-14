using E_commerce.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DTO_s.Product
{
    public class ProductCreateRequest
    {
        [Required(ErrorMessage = "Title is requiered!")]
        [StringLength(50, ErrorMessage = "Title must be between 3 and 40 characters.", MinimumLength = 3)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is requiered!")]
        [StringLength(50, ErrorMessage = "Description must be between 20 and 500 characters.", MinimumLength = 3)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is requiered!")]
        [Range(1, 100000000, ErrorMessage = "Price needs to be bewteen 1 - 100 000 000 Sek")]
        public int Price { get; set; }
        public DateTime DateListed { get; set; } = DateTime.Now;
        public Guid OrderId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
