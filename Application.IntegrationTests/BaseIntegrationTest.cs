using E_commerce.Context;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests
{
    /// <summary>
    /// BaseIntegrationTest serves as a foundation for integration tests, providing access to essential
    /// services utilized across multiple test cases. 
    /// </summary>
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestFactory>
    {
        private readonly IServiceScope _scope;
        protected readonly ISender Sender;
        protected BaseIntegrationTest(IntegrationTestFactory factory)
        {
            _scope = factory.Services.CreateScope();

            Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        }
    }
}
