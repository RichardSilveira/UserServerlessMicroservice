using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UserService;
using UserService.Domain.Requests;
using UserService.Functions;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Repositories.Transactions;
using UserServiceTests.Infrastructure;
using Xunit;

namespace UserServiceTests
{
    public class UserIntegrationTests
    {
        [Fact]
        public async Task AddNew_ValidUser_Via_LocalMySql_Should_Returns_201Created()
        {
            // Arange
            var configuration = ConfigurationService.BuildConfiguration("local");

            var proxy = new APIGatewayHttpApiV2ProxyRequest();

            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            optionsBuilder.UseMySql(configuration["UserServiceDbContextConnectionString"]);

            var localMySqlDbCtxt = new UserContext(optionsBuilder.Options);
            UserContextInitializer.ClearDatabase(localMySqlDbCtxt);

            var userRepository = new UserRepository(localMySqlDbCtxt);
            var userQueryService = new UserQueryService(localMySqlDbCtxt);
            var unitOfWork = new UnitOfWork(localMySqlDbCtxt, new NoMediator());

            var addUserRequest = new AddUserRequest()
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "newvalidemail@email.com",
                Address = new AddressRequest()
                {
                    Country = "Argentina",
                    Street = "Buenos Aires"
                }
            };

            proxy.Body = JsonSerializer.Serialize(addUserRequest);

            // Act
            var function = new AddNewUserFunction(configuration, unitOfWork, userRepository, userQueryService);
            var result = await function.Handle(proxy, new TestLambdaContext());

            // Assert
            result.StatusCode.Should().Be((int) HttpStatusCode.Created);
        }
    }
}