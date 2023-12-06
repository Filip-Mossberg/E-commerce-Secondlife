using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DbModels
{
    public class Image
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public bool IsDisplayImage { get; set; }
        public int ProductId { get; set; }
    }
}
