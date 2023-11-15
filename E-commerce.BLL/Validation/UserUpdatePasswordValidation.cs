using E_commerce.Models.DTO_s.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.Validation
{
    public class UserUpdatePasswordValidation : AbstractValidator<UserUpdatePasswordRequest>
    {
        public UserUpdatePasswordValidation()
        {
            // The email is sent with the UserUpdateOasswordRequest automatically
            RuleFor(email => email.Email).NotEmpty();
            RuleFor(password => password.PasswordHash).NotEmpty().WithMessage("Password is requierd!")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(128).WithMessage("Password cannot exceed 128 characters.")
                .Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).*$").WithMessage("Password needs to contain at least one uppercase letter, one digit, and one special character.");
            RuleFor(confPassword => confPassword.ConfirmPassword)
                .Equal(password => password.PasswordHash).WithMessage("Passwords do not match.");
            RuleFor(currentPassword => currentPassword.CurrentPassword).NotEmpty().WithMessage("Current Password field cannot be empty");
        }
    }
}
