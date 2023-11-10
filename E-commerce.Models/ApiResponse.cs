using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Errors = new List<string>();
        }
        public bool IsSuccess { get; set; }
        public Object Result { get; set; }
        public List<string> Errors { get; set; }
    }
}
