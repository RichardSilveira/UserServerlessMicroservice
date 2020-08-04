using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Configuration;
using UserService.Domain;
using UserService.Extensions;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Repositories.Transactions;
using static System.Text.Json.JsonSerializer;

// If targeting .NET Core 3.1 this serializer is highly recommend over Amazon.Lambda.Serialization.Json and can significantly reduce cold start performance in Lambda.
[assembly:
    Amazon.Lambda.Core.LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace UserService.Functions
{
    public class AddNewUserFunction : FunctionBase
    {
        private IConfiguration _configuration;
        private IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;

        private SomeUserDomainService _userDomainService;


        private void Configure()
        {
            LambdaLogger.Log("Configure Starts");
            LambdaLogger.Log("RunningAsLocal?" + RunningAsLocal);
            LambdaLogger.Log("_unitOfWork:" + (_unitOfWork == null).ToString());
            LambdaLogger.Log("_userRepository:" + (_userRepository == null).ToString());
            LambdaLogger.Log("_userDomainService:" + (_userDomainService == null).ToString());
            _configuration = new ConfigurationService().GetConfiguration();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            _unitOfWork = serviceProvider.GetService<IUnitOfWork>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _userDomainService = serviceProvider.GetService<SomeUserDomainService>();
            LambdaLogger.Log("Configure after injection");
            LambdaLogger.Log("_unitOfWork:" + (_unitOfWork == null).ToString());
            LambdaLogger.Log("_userRepository:" + (_userRepository == null).ToString());
            LambdaLogger.Log("_userDomainService:" + (_userDomainService == null).ToString());
        }

        public AddNewUserFunction()
        {
            // Parameterless constructor required by AWS Lambda runtime 
        }

        /* You need pass all your abstractions here to have them injected for tests.
         This way neither the ConfigureServices nor the Configure won't be called.
         */
        public AddNewUserFunction(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            SomeUserDomainService userDomainService)
        {
            RunningAsLocal = true;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userDomainService = userDomainService;
        }


        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            var connString = _configuration["UserServiceDbContextConnectionString"];

            serviceCollection.AddDbContext<UserServiceDbContext>(options => options.UseMySql(connString));

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<SomeUserDomainService>();
        }

        public APIGatewayHttpApiV2ProxyResponse Handle(APIGatewayHttpApiV2ProxyRequest request,
            ILambdaContext context)
        {
            LambdaLogger.Log($"CONTEXT {Serialize(context.GetMainProperties())}");
            LambdaLogger.Log($"EVENT: {Serialize(request.GetMainProperties())}");

            if (!RunningAsLocal)
                Configure();


            var userRequest = JsonSerializer.Deserialize<AddUserRequest>(request.Body);

            var address = new Address(userRequest.Country, userRequest.Street, userRequest.City,
                userRequest.State);

            var user = new User(userRequest.FirstName, userRequest.LastName);
            user.UpdateAddressInfo(address);

            _userRepository.Add(user);
            _unitOfWork.SaveChanges();


            return Created(options =>
            {
                options.Body = Serialize(user);
                options.Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"},
                    {"Location", $"https://e8teskfbxf.execute-api.us-east-1.amazonaws.com/dev/v1/users/{user.Id}"}
                };
            });
        }

        //TODO: Handling bad request responses
    }
}