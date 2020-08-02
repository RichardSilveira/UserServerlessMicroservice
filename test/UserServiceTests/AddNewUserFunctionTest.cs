using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Microsoft.Extensions.Configuration;
using Moq;
using UserService;
using UserService.Configuration;
using UserService.Domain;
using UserService.Functions;
using UserServiceTests.Infrastructure;
using Xunit;

namespace UserServiceTests
{
    public class AddNewUserFunctionTest
    {
        private IConfiguration _configuration;

        public AddNewUserFunctionTest()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.test.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }


        [Fact]
        public async Task AddNewValidUser_Should_Returns_201_StatusCode()
        {
            // Should mock the Stage variable that is created by serverless framework
            var stage = "dev";
            var mockConfig = new Mock<IEnvironmentService>();
            mockConfig.Setup(p => p.EnvironmentName).Returns(stage);

            var proxy = new APIGatewayHttpApiV2ProxyRequest();

            var user = new User("Foo", "Bar");
            proxy.Body = JsonSerializer.Serialize(user);

            var userRepository = new UserRepositoryInMemoryStub();
            var userDomainService = new SomeUserDomainService(userRepository);
            var function = new AddNewUserFunction(_configuration, userRepository, userDomainService);
            var result = await function.Handle(proxy, new TestLambdaContext());

            Assert.True(result.StatusCode == (int) HttpStatusCode.Created);
        }
    }
}