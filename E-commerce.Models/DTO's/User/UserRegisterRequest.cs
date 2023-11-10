using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Models.DTO_s.User
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "Username is requiered!")]
        [StringLength(50, ErrorMessage = "Username must be between 3 and 40 characters.", MinimumLength = 3)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is requiered!")]
        [EmailAddress(ErrorMessage = "Invalid email adress format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is requiered!")]
        [RegularExpression("^(?=.*\\d)(?=.*[A-Z])(?=.*\\W).+", ErrorMessage = "Password needs to contain at least one uppercase letter, one digit, and one special character.")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Confirm Password is requiered!")]
        [DataType(DataType.Password)]
        [Compare("PasswordHash", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
