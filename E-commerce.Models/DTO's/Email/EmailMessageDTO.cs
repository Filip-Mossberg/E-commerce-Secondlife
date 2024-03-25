using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DTO_s.Email
{
    public class EmailMessageDTO
    {
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
