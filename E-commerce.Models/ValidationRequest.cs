using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models
{
    public class ValidationRequest
    {
        public bool Valid { get; set; }
        public List<string> Errors { get; set; }
    }
}
