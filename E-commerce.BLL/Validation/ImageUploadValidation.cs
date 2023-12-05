using E_commerce.Models.DTO_s.Image;
using FluentValidation;

namespace E_commerce.BLL.Validation
{
    public class ImageUploadValidation : AbstractValidator<ImageUploadRequest>
    {
        public ImageUploadValidation()
        {
            RuleFor(path => path.FilePath)
                .NotEmpty().WithMessage("FilePath is required!")
                .Must(ImageFormatCheck).WithMessage("File format must be either PNG, JPEG, or JPG.");
        }

        private bool ImageFormatCheck(string path)
        {
            if(path.ToLower().Contains(".png") || path.ToLower().Contains(".jpeg") || path.ToLower().Contains(".jpg"))
            {
                return true;
            }

            return false;
        }
    }
}
