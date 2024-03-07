using E_commerce.Context;
using E_commerce.Models.DbModels;
using E_commerce_BLL.IService;
using E_commerce_BLL.Service;
using E_commerce_DAL.IRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationTests
{
    /// <summary>
    /// Base class for integration tests
    /// </summary>
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestFactory>
    {
        private readonly IServiceScope _scope;
        protected readonly ISender Sender;
        protected readonly AppDbContext _context;
        protected BaseIntegrationTest(IntegrationTestFactory factory)
        {
            _scope = factory.Services.CreateScope();

            Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
            _context = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }
    }
}
