using E_commerce.Models.DTO_s.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.Validation
{
    public class ProductCreateValidation : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateValidation()
        {
            RuleFor(title => title.Title)
                .NotEmpty().WithMessage("Title is requierd!")
                .Length(3, 50).WithMessage("Title must be between 3 and 40 characters.");
            RuleFor(desc => desc.Description)
                .NotEmpty().WithMessage("Description is requierd!")
                .Length(50, 500).WithMessage("Description must be between 50 and 500 characters.");
            RuleFor(price => price.Price)
                .NotEmpty().WithMessage("Price is requierd!")
                .ExclusiveBetween(1, 100000000).WithMessage("Price needs to be bewteen 1 - 100 000 000 Sek");
            RuleFor(image => image.Images)
                .NotEmpty().WithMessage("One image is required!");
        }
    }
}



