using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Configuration;
using UserService.Domain;
using static System.Text.Json.JsonSerializer;

// If targeting .NET Core 3.1 this serializer is highly recommend over Amazon.Lambda.Serialization.Json and can significantly reduce cold start performance in Lambda.
[assembly:
    Amazon.Lambda.Core.LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace UserService.Functions
{
    public class AddNewUserFunction : FunctionBase
    {
        private SomeUserDomainService _userDomainService;
        private IUserRepository _userRepository;

        // Invoked by AWS Lambda at runtime
        public AddNewUserFunction()
        {
        }

        /* You need pass all your abstractions here to have them injected for tests.
         This way neither the ConfigureServices nor the Configure won't be called.
         */
        public AddNewUserFunction(
            IConfiguration configuration,
            IUserRepository userRepository,
            SomeUserDomainService userDomainService) : base(configuration)
        {
            _userRepository = userRepository;
            _userDomainService = userDomainService;
        }


        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IEnvironmentService, EnvironmentService>();
            serviceCollection.AddTransient<IConfigurationService, ConfigurationService>();

            // Other injections here
            serviceCollection.AddScoped<IUserRepository, UserRepositoryInMemory>();
            serviceCollection.AddScoped<SomeUserDomainService>();
        }

        protected override void Configure(IServiceProvider serviceProvider)
        {
            base.Configure(serviceProvider);
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _userDomainService = serviceProvider.GetService<SomeUserDomainService>();
        }

        public async Task<APIGatewayHttpApiV2ProxyResponse> Handle(APIGatewayHttpApiV2ProxyRequest request,
            ILambdaContext context)
        {
            LambdaLogger.Log($"CONTEXT {Serialize(context.GetMainProperties())}");
            LambdaLogger.Log($"EVENT: {Serialize(request.GetMainProperties())}");

            await Task.CompletedTask; //todo: I'll replace it

            var userRetrieved = _userRepository.GetUser();
            LambdaLogger.Log("userRetrieved: " + Serialize(userRetrieved));

            var isValid = _userDomainService.DoSomeLogicInvolvingUser();
            LambdaLogger.Log("isValid: " + isValid);

            var env1Value = Configuration["envVar1"];
            var env2Value = Configuration["envVar2"];
            LambdaLogger.Log($"envVar1: {env1Value}, envVar2: {env2Value}");

            var userRequest = JsonSerializer.Deserialize<UserRequest>(request.Body);

            var user = new User(userRequest.FirstName, userRequest.LastName);

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