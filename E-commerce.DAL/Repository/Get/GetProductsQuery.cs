using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.DAL.Repository.Get
{
    // A record that consolidates product retrieval parameters into a single object for improved code organization and readability.
    public record GetProductsQuery(
        string? 
        SearchTerm, 
        string? SortColumn, 
        string? SortOrder, 
        int? Category,
        int Page, 
        int PageSize) : IRequest<PageList<ProductGetResponse>>;
}
