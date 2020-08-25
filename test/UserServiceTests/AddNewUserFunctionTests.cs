using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using UserService;
using UserService.Configuration;
using UserService.Domain;
using UserService.Domain.Events;
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
        public async Task AddNewValidUser_Should_Returns_201Created()
        {
            // Arrange
            var configuration = ConfigurationService.BuildConfiguration("local");

            var unitOfWork = new UnitOfWorkInMemory();
            var userRepository = new UserRepositoryInMemory();
            var userQueryServiceMock = new Mock<IUserQueryService>();
            userQueryServiceMock.Setup(p => p.GetUsersByEmail("newvalidemail@email.com"))
                .Returns(Task.FromResult<IEnumerable<User>>(new List<User>()));

            var proxy = new APIGatewayHttpApiV2ProxyRequest() {Body = JsonSerializer.Serialize(AddUserRequest.Factory.ValidUserSample())};

            // Act
            var function = new AddNewUserFunction(configuration, unitOfWork, userRepository, userQueryServiceMock.Object);
            var result = await function.Handle(proxy, new TestLambdaContext());

            // Assert
            result.StatusCode.Should().Be((int) HttpStatusCode.Created);
        }

        [Fact]
        public async Task AddUser_Succesfully_Should_Publish_UserRegisteredEvent()
        {
            // Arrange
            var configuration = ConfigurationService.BuildConfiguration("local");

            var context = UserContext.Factory.CreateNew(options => options.UseInMemoryDatabase(databaseName: "InMemory"));
            UserContextInitializer.ClearDatabase(context);

            var mediatorMock = new Mock<IMediator>();
            var unitOfWork = new UnitOfWork(context, mediatorMock.Object);
            var userRepository = new UserRepository(context);
            var userQueryService = new UserQueryService(context);

            var proxy = new APIGatewayHttpApiV2ProxyRequest() {Body = JsonSerializer.Serialize(AddUserRequest.Factory.ValidUserSample())};

            // Act
            var function = new AddNewUserFunction(configuration, unitOfWork, userRepository, userQueryService);
            var result = await function.Handle(proxy, new TestLambdaContext());

            // Assert
            mediatorMock.Verify(x =>
                x.Publish(It.Is<INotification>(it => it.GetType() == typeof(UserRegisteredDomainEvent)), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task AddUser_Should_Returns_BadRequest_When_Email_AlreadyExists()
        {
            // Arrange
            var configuration = ConfigurationService.BuildConfiguration("local");

            var context = UserContext.Factory.CreateNew(options => options.UseInMemoryDatabase(databaseName: "InMemory"));
            var seedResult = UserContextInitializer.SeedDatabase(context);


            var unitOfWork = new UnitOfWork(context, new NoMediator());
            var userRepository = new UserRepository(context);
            var userQueryService = new UserQueryService(context);

            var userRequest = AddUserRequest.Factory.ValidUserSample();
            userRequest.Email = seedResult.Users[0].Email; // Setting up a random existing email

            var proxy = new APIGatewayHttpApiV2ProxyRequest() {Body = JsonSerializer.Serialize(userRequest)};

            // Act
            var function = new AddNewUserFunction(configuration, unitOfWork, userRepository, userQueryService);
            var result = await function.Handle(proxy, new TestLambdaContext());

            // Assert
            result.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddUser_Should_Contains_Country_When_Address_Is_Informed()
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
    }
}