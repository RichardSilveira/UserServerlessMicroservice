using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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
using UserService.Domain.Requests;
using UserService.Functions;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Repositories.Transactions;
using UserServiceTests.Infrastructure;
using Xunit;

namespace UserServiceTests
{
    public class AddNewUserFunctionTest
    {
        [Fact]
        public async Task AddNewValidUser_Should_Returns_Created()
        {
            var configuration = ConfigurationService.BuildConfiguration("local");
            var proxy = new APIGatewayHttpApiV2ProxyRequest();

            var addUserRequest = new AddUserRequest()
            {
                FirstName = "Julia",
                LastName = "Doe",
                Email = "newvalidemail@email.com",
                Address = new AddressRequest()
                {
                    Country = "Brazil",
                    Street = "Flower St."
                }
            };

            proxy.Body = JsonSerializer.Serialize(addUserRequest);

            var unitOfWork = new UnitOfWorkInMemory();
            var userRepository = new UserRepositoryInMemory();

            var userQueryServiceMock = new Mock<IUserQueryService>();
            userQueryServiceMock.Setup(p => p.GetUsersByEmail("newvalidemail@email.com")).Returns(Task.FromResult<IEnumerable<User>>(null));

            var function = new AddNewUserFunction(configuration, unitOfWork, userRepository, userQueryServiceMock.Object);

            var result = await function.Handle(proxy, new TestLambdaContext());

            Assert.True(result.StatusCode == (int) HttpStatusCode.Created);
        }

        [Fact]
        public async Task AddUser_EmailAlreadyExists_Should_Returns_BadRequest()
        {
            await Task.CompletedTask;
        }

        [Fact]
        public async Task If_Address_Is_Informed_Should_Contains_Country()
        {
            await Task.CompletedTask;
        }

        #region Validations against required fields

        [Fact]
        public async Task AddUser_WithoutEmail_Should_Returns_BadRequest()
        {
            await Task.CompletedTask;
        }

        [Fact]
        public async Task AddUser_WithoutFirstName_Should_Returns_BadRequest()
        {
            await Task.CompletedTask;
        }

        [Fact]
        public async Task AddUser_WithoutLastName_Should_Returns_BadRequest()
        {
            await Task.CompletedTask;
        }

        #endregion

        #region Validations againts domain events

        //todo: 

        #endregion
    }
}