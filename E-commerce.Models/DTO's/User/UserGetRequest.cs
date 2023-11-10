using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DTO_s.User
{
    public class UserGetRequest
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
