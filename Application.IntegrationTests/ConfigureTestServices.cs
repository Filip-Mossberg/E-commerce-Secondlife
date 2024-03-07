using E_commerce_BLL.IService;
using E_commerce_BLL.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IntegrationTests
{
    public static class ConfigureTestServices
    {
        public static IServiceCollection DbServicesTest(this IServiceCollection service)
        {

            return service;
        }
    }
}
