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
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Repositories.Transactions;
using static System.Text.Json.JsonSerializer;

// If targeting .NET Core 3.1 this serializer is highly recommend over Amazon.Lambda.Serialization.Json and can significantly reduce cold start performance in Lambda.
[assembly:
    Amazon.Lambda.Core.LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace UserService.Functions
{
    public class AddNewUserFunction
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        private SomeUserDomainService _userDomainService;


        // Invoked by AWS Lambda at runtime
        public AddNewUserFunction()
        {
            _configuration = new ConfigurationService().GetConfiguration();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            _unitOfWork = serviceProvider.GetService<IUnitOfWork>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _userDomainService = serviceProvider.GetService<SomeUserDomainService>();
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

            var userRequest = JsonSerializer.Deserialize<AddUserRequest>(request.Body);

            var address = new Address(userRequest.Country, userRequest.Street, userRequest.City,
                userRequest.State);

            var user = new User(userRequest.FirstName, userRequest.LastName);
            user.UpdateAddressInfo(address);

            _userRepository.Add(user);
            _unitOfWork.SaveChanges();

            var response = new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = (int) HttpStatusCode.Created,
                Body = Serialize(user),
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"},
                    {"Location", $"https://e8teskfbxf.execute-api.us-east-1.amazonaws.com/dev/v1/users/{user.Id}"}
                }
            };
            // Location Header should be set after you've a dns name

            return response;
        }

        //TODO: Handling bad request responses
    }
}