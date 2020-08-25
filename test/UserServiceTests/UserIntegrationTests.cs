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
            // Arrange
            var configuration = ConfigurationService.BuildConfiguration("local");

            var context = UserContext.Factory.CreateNew(options => options.UseMySql(configuration["UserServiceDbContextConnectionString"]));
            UserContextInitializer.ClearDatabase(context);

            var unitOfWork = new UnitOfWork(context, new NoMediator());
            var userRepository = new UserRepository(context);
            var userQueryService = new UserQueryService(context);

            var proxy = new APIGatewayHttpApiV2ProxyRequest() {Body = JsonSerializer.Serialize(AddUserRequest.Factory.ValidUserSample())};

            // Act
            var function = new AddNewUserFunction(configuration, unitOfWork, userRepository, userQueryService);
            var result = await function.Handle(proxy, new TestLambdaContext());

            // Assert
            result.StatusCode.Should().Be((int) HttpStatusCode.Created);
        }
    }
}