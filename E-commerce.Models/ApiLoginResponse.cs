using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models
{
    public class ApiLoginResponse
    {
        public ApiLoginResponse()
        {
            Errors = new List<string>();
        }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public bool EmailConfirmed { get; set; }
        public Object Result { get; set; }
        public List<string> Errors { get; set; }
    }
}
