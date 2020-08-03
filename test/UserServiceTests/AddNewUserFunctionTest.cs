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
            var stage = "test";
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
        public void AddNewValidUser_Should_Returns_201_StatusCode()
        {
            var proxy = new APIGatewayHttpApiV2ProxyRequest();

            var addUserRequest = new AddUserRequest()
            {
                FirstName = "Julia",
                LastName = "Doe",
                Country = "Brazil",
                Street = "Flower St."
            };

            proxy.Body = JsonSerializer.Serialize(addUserRequest);

            var unitOfWork = new UnitOfWorkInMemory();
            var userRepository = new UserRepositoryInMemory();

            var function =
                new AddNewUserFunction(_configuration, unitOfWork, userRepository,
                    new SomeUserDomainService(userRepository));

            var result = function.Handle(proxy, new TestLambdaContext());

            Assert.True(result.StatusCode == (int) HttpStatusCode.Created);
        }

        [Fact]
        public void AddNewUser_Via_LocalMySql()
        {
            var proxy = new APIGatewayHttpApiV2ProxyRequest();

            var addUserRequest = new AddUserRequest()
            {
                FirstName = "John",
                LastName = "Doe",
                Country = "Argentina",
                State = "Buenos Aires"
            };

            proxy.Body = JsonSerializer.Serialize(addUserRequest);

            var optionsBuilder = new DbContextOptionsBuilder<UserServiceDbContext>();
            optionsBuilder.UseMySql(
                _configuration["UserServiceDbContextConnectionString"]);

            var localMySqlDbCtxt = new UserServiceDbContext(optionsBuilder.Options);

            var userRepository = new UserRepository(localMySqlDbCtxt);
            var unitOfWork = new UnitOfWork(localMySqlDbCtxt);
            var userDomainService = new SomeUserDomainService(userRepository);
            var function = new AddNewUserFunction(_configuration, unitOfWork, userRepository, userDomainService);
            var result = function.Handle(proxy, new TestLambdaContext());

            Assert.True(result.StatusCode == (int) HttpStatusCode.Created);
        }
    }
}