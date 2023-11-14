using E_commerce.Models.DTO_s.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.Validation
{
    public class UserRegisterValidation : AbstractValidator<UserRegisterRequest>
    {
        public UserRegisterValidation()
        {
            RuleFor(email => email.Email)
                .NotEmpty().WithMessage("Email is required!")
                .EmailAddress().WithMessage("Invalid email adress format.");
            RuleFor(password => password.PasswordHash)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(128).WithMessage("Password cannot exceed 128 characters.")
                .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).*$").WithMessage("Password needs to contain at least one uppercase letter, one digit, and one special character.");
            RuleFor(confPassword => confPassword.ConfirmPassword)
                .Matches("PasswordHash").WithMessage("Passwords do not match.");
        }
    }
}
