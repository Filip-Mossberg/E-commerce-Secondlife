using E_commerce.Models.DTO_s.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.Validation
{
    internal class UserUpdateRequestValidation : AbstractValidator<UserUpdateRequest>
    {
        public UserUpdateRequestValidation()
        {
            When(user => user.UserName != null, () =>
            {
                RuleFor(u => u.UserName)
                .Length(3, 50).WithMessage("Username must be between 3 and 40 characters.");
            });

            When(user => user.Email != null, () =>
            {
                RuleFor(u => u.Email)
                .EmailAddress().WithMessage("Invalid email adress format.");
            });

            When(user => user.Password != null, () =>
            {
                RuleFor(u => u.Password)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(128).WithMessage("Password cannot exceed 128 characters.")
                .Matches("^(?=.*\\d)(?=.* [A - Z])(?=.*\\W).+").WithMessage("Password needs to contain at least one uppercase letter, one digit, and one special character.");

                RuleFor(u => u.ConfirmPassword)
                .Equal(u => u.Password).WithMessage("Passwords do not match.");
            });
        }
    }
}