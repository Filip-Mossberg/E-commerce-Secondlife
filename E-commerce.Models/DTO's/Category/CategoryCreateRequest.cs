using MediatR;

namespace E_commerce.Models.DTO_s.Category
{
    public class CategoryCreateRequest : IRequest<ApiResponse>
    {
        public string Name { get; set; }

        public CategoryCreateRequest(string Name)
        {
            this.Name = Name;
        }
    }
}
