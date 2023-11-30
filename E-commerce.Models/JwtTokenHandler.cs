using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models
{
    public class JwtTokenHandler
    {
        public JwtTokenHandler(JwtSecurityToken token, DateTime expireDate)
        {
            JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            ExpireDate = expireDate;
        }

        public string JwtToken { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
