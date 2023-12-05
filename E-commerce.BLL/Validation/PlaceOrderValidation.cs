using E_commerce.Models.DTO_s.Order;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.Validation
{
    public class PlaceOrderValidation : AbstractValidator<PlaceOrderRequest>
    {
        public PlaceOrderValidation()
        {
            RuleFor(products => products.Products)
               .NotEmpty().WithMessage("Order cant be empty!");
            RuleFor(email => email.Email)
                .NotEmpty().WithMessage("Email cant be empty!");
            RuleFor(user => user.UserId)
                .NotEmpty().WithMessage("Error placing order!");
        }
    }
}
