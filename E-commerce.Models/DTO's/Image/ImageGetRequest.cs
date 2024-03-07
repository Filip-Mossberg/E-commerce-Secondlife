using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DTO_s.Image
{
    public class ImageGetRequest
    {
        public ImageGetRequest()
        {
            
        }

        public ImageGetRequest(string Url, bool IsDisplayImage, string Id)
        {
            this.Id = Id;
            this.Url = Url;
            this.IsDisplayImage = IsDisplayImage;
        }

        public string Id { get; set; }
        public string Url { get; set; }
        public bool IsDisplayImage { get; set; }
    }
}
