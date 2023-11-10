using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DbModels
{
    public class Image
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public bool IsDisplayImage { get; set; }
        public int ProductId { get; set; }
        public Product product { get; set; }
    }
}
