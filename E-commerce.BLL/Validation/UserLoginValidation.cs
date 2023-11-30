using E_commerce.Models.DTO_s.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.Validation
{
    public class UserLoginValidation : AbstractValidator<UserLoginRequest>
    {
        public UserLoginValidation()
        {
            RuleFor(email => email.Email)
                .NotEmpty().WithMessage("Email field required!");
            RuleFor(password => password.Password)
                .NotEmpty().WithMessage("Password field required");
        }
    }
}
