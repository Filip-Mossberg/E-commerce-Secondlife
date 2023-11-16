using E_commerce.Models.DTO_s.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.Validation
{
    public class CategoryCreateValidation : AbstractValidator<CategoryCreateRequest>
    {
        public CategoryCreateValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category name is required!")
                .Length(2, 20).WithMessage("Category name must be between 3 and 40 characters.");
        }
    }
}
