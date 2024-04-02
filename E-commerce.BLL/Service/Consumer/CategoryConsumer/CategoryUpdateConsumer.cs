using E_commerce.DAL.IRepository;
using E_commerce.Models.DbModels;
using MassTransit;

namespace E_commerce.BLL.Service.Consumer.CategoryConsumer
{
    public class CategoryUpdateConsumer : IConsumer<Category>
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryUpdateConsumer(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task Consume(ConsumeContext<Category> context)
        {
            await _categoryRepository.UpdateCategory(context.Message);
        }
    }
}
