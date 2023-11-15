using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DTO_s.User
{
    public class UserUpdatePasswordRequest
    {
        public string Email { get; set; }
        public string ConfirmPassword { get; set; }
        public string CurrentPassword { get; set; }
    }
}
