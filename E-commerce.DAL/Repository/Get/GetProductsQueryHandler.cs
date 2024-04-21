using E_commerce.Context;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Image;
using E_commerce.Models.DTO_s.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq.Expressions;

namespace E_commerce.DAL.Repository.Get
{
    internal sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PageList<ProductGetResponse>>
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;

        public GetProductsQueryHandler(AppDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<PageList<ProductGetResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Product> productsQuery = _context.Product;

            // Applying filtering
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                productsQuery = productsQuery.Where(p => p.Title.ToLower().Contains(request.SearchTerm.ToLower()));
            }
            if ((request.Category ?? 0) != 0)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == request.Category);
            }

            // Applying sorting
            if (request.SortOrder?.ToLower() == "desc")
            {
                productsQuery = productsQuery.OrderByDescending(GetSortProperty(request));
            }
            else if (request.SortOrder?.ToLower() == "asc")
            {
                productsQuery = productsQuery.OrderBy(GetSortProperty(request));
            }
            else
            {
                productsQuery = productsQuery.OrderBy(p => p.RandomOrderIdentifier);
            }

            // Applying projection
            var productResponsesQuery = productsQuery
                .Include(p => p.Images)
                .Select(p => new ProductGetResponse(
                    p.Id,
                    p.Title,
                    p.Description,
                    p.Category.Name,
                    p.Price,
                    p.DateListed,
                    p.Images.Select(i => new ImageGetRequest
                    {
                        Id = i.Id,
                        Url = i.Url,
                        IsDisplayImage = i.IsDisplayImage
                    }).ToList()
                    )); ;

            // Pagination at database level returning PageList<ProductResponse> 
            var products = await PageList<ProductGetResponse>.CreateAsync(
                productResponsesQuery,
                request.Page,
                request.PageSize);

            return products;

        }

        /// <summary>
        /// The GetSortProperty method returns a lambda expression based on the SortColumn propery from the request
        /// </summary>
        private static Expression<Func<Product, object>> GetSortProperty(GetProductsQuery request)
        {
            return request.SortColumn?.ToLower() switch
            {
                "title" => product => product.Title,
                "price" => product => product.Price,
                _ => product => product.RandomOrderIdentifier
            }; ;
        }
    }
}
