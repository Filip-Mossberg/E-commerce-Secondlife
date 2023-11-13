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
        [Required]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is requiered!")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [MaxLength(128, ErrorMessage = "Password cannot exceed 128 characters.")]
        [RegularExpression("^(?=.*\\d)(?=.*[A-Z])(?=.*\\W).+", ErrorMessage = "Password needs to contain at least one uppercase letter, one digit, and one special character.")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Confirm Password is requiered!")]
        [DataType(DataType.Password)]
        [Compare("PasswordHash", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string CurrentPassword { get; set; }
    }
}
