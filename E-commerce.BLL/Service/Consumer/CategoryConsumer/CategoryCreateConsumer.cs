using AutoMapper;
using E_commerce.Context;
using E_commerce.DAL.IRepository;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Category;
using MassTransit;
using Serilog;

namespace E_commerce.BLL.Service.Consumer.CategoryConsumer
{
    public class CategoryCreateConsumer : IConsumer<CategoryCreateRequest>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryCreateConsumer(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;   
        }
        public async Task Consume(ConsumeContext<CategoryCreateRequest> context)
        {
            await _categoryRepository.CreateCategory(_mapper.Map<Category>(context.Message));
        }
    }
}
