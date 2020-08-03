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
    public class AddNewUserFunction : FunctionBase
    {
        private IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;

        private SomeUserDomainService _userDomainService;


        // Invoked by AWS Lambda at runtime
        public AddNewUserFunction()
        {
        }

        /* You need pass all your abstractions here to have them injected for tests.
         This way neither the ConfigureServices nor the Configure won't be called.
         */
        public AddNewUserFunction(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            SomeUserDomainService userDomainService) : base(configuration)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userDomainService = userDomainService;
        }


        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IEnvironmentService, EnvironmentService>();
            serviceCollection.AddTransient<IConfigurationService, ConfigurationService>();

            // Other injections goes here
            var optionsBuilder = new DbContextOptionsBuilder<UserServiceDbContext>();
            optionsBuilder.UseMySql(
                Configuration["ConnectionStrings:UserServiceDbContext"]);

            serviceCollection.AddScoped<UserServiceDbContext>((provider =>
                new UserServiceDbContext(optionsBuilder.Options)));

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<SomeUserDomainService>();
        }

        protected override void Configure(IServiceProvider serviceProvider)
        {
            var userServiceContext = serviceProvider.GetService<UserServiceDbContext>();
            _unitOfWork = serviceProvider.GetService<IUnitOfWork>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _userDomainService = serviceProvider.GetService<SomeUserDomainService>();
        }

        public async Task<APIGatewayHttpApiV2ProxyResponse> Handle(APIGatewayHttpApiV2ProxyRequest request,
            ILambdaContext context)
        {
            LambdaLogger.Log($"CONTEXT {Serialize(context.GetMainProperties())}");
            LambdaLogger.Log($"EVENT: {Serialize(request.GetMainProperties())}");

            var userRetrieved = await _userRepository.GetById(Guid.Empty);
            LambdaLogger.Log("userRetrieved: " + Serialize(userRetrieved));

            var isValid = _userDomainService.DoSomeLogicInvolvingUser();
            LambdaLogger.Log("isValid: " + isValid);

            var env1Value = Configuration["envVar1"];
            var env2Value = Configuration["envVar2"];
            LambdaLogger.Log($"envVar1: {env1Value}, envVar2: {env2Value}");

            var userRequest = JsonSerializer.Deserialize<UserRequest>(request.Body);

            var user = new User(userRequest.FirstName, userRequest.LastName);

            _userRepository.Add(user);
            _unitOfWork.SaveChanges();
            //todo: dispose connection

            var response = new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = (int) HttpStatusCode.Created,
                Body = Serialize(user),
                Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}}
            };
            //todo: Add location Header

            return response;
        }
    }
}