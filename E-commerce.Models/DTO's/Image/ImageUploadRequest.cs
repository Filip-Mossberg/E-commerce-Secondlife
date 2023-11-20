using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DTO_s.Image
{
    public class ImageUploadRequest
    {
        public string FilePath { get; set; }
        public bool IsDisplayImage { get; set; }
    }
}
