using E_commerce.Models.DbModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_commerce.DAL.Repository.Get
{
    public class PageList<T>
    {
        // Private constructor enforce the use of CreateAsync when creating instances of PageList<T>
        [JsonConstructor] // Tells the json deserializer to use this constructor for deserialization.
        public PageList(List<T> items, int page, int pageSize, int totalCount)
        {
            this.items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public List<T> items { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public bool HasNextPage => Page * PageSize < TotalCount;
        public bool HasPrevious => PageSize > 1;

        /// <summary>
        /// Factory method used to create instances of PageList<T>
        /// </summary>
        /// <returns>
        /// A new instance of PageList<T>
        /// </returns>
        public static async Task<PageList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize)
        {
            int totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new(items, page, pageSize, totalCount);
        }
    }
}
