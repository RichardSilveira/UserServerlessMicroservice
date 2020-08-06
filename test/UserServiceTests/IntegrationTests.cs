﻿using System;
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
    public class IntegrationTests
    {
        [Fact]
        public async Task AddNewUser_Via_LocalMySql()
        {
            var configuration = ConfigurationService.BuildConfiguration("local");

            var proxy = new APIGatewayHttpApiV2ProxyRequest();

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

            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            optionsBuilder.UseMySql(configuration["UserServiceDbContextConnectionString"]);

            var localMySqlDbCtxt = new UserContext(optionsBuilder.Options);

            var userRepository = new UserRepository(localMySqlDbCtxt);
            var userQueryService = new UserQueryService(localMySqlDbCtxt);
            var unitOfWork = new UnitOfWork(localMySqlDbCtxt);

            var function = new AddNewUserFunction(configuration, unitOfWork, userRepository, userQueryService);

            var result = await function.Handle(proxy, new TestLambdaContext());

            Assert.True(result.StatusCode == (int) HttpStatusCode.Created);
        }
    }
}