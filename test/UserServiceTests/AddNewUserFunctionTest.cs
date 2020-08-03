using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using UserService;
using UserService.Configuration;
using UserService.Domain;
using UserService.Functions;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Repositories.Transactions;
using UserServiceTests.Infrastructure;
using Xunit;

namespace UserServiceTests
{
    public class AddNewUserFunctionTest
    {
        private IConfiguration _configuration;

        public AddNewUserFunctionTest()
        {
            // Should mock the Stage variable that is created by serverless framework
            var stage = "prod";
            var mockConfig = new Mock<IEnvironmentService>();
            mockConfig.Setup(p => p.EnvironmentName).Returns(stage);

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{mockConfig.Object.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }


        [Fact]
        public async Task AddNewValidUser_Should_Returns_201_StatusCode()
        {
            var proxy = new APIGatewayHttpApiV2ProxyRequest();

            var user = new User("Foo", "Bar");
            proxy.Body = JsonSerializer.Serialize(user);

            var userRepository = new UserRepositoryInMemory();
            var unitOfWork = new UnitOfWorkInMemory();
            var userDomainService = new SomeUserDomainService(userRepository);
            var function = new AddNewUserFunction(_configuration, unitOfWork, userRepository, userDomainService);
            var result = await function.Handle(proxy, new TestLambdaContext());

            Assert.True(result.StatusCode == (int) HttpStatusCode.Created);
        }

        [Fact]
        public async Task AddNewUser_Via_LocalMySql()
        {
            var proxy = new APIGatewayHttpApiV2ProxyRequest();

            var user = new User("John", "Doe");
            proxy.Body = JsonSerializer.Serialize(user);

            var optionsBuilder = new DbContextOptionsBuilder<UserServiceDbContext>();
            optionsBuilder.UseMySql(
                _configuration["ConnectionStrings:UserServiceDbContext"]);

            var localMySqlDbCtxt = new UserServiceDbContext(optionsBuilder.Options);

            var userRepository = new UserRepository(localMySqlDbCtxt);
            var unitOfWork = new UnitOfWork(localMySqlDbCtxt);
            var userDomainService = new SomeUserDomainService(userRepository);
            var function = new AddNewUserFunction(_configuration, unitOfWork, userRepository, userDomainService);
            var result = await function.Handle(proxy, new TestLambdaContext());

            Assert.True(result.StatusCode == (int) HttpStatusCode.Created);
        }
    }
}