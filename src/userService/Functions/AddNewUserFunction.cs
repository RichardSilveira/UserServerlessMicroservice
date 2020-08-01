using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using UserService.Configuration;
using UserService.Domain;

// If targeting .NET Core 3.1 this serializer is highly recommend over Amazon.Lambda.Serialization.Json and can significantly reduce cold start performance in Lambda.
[assembly:
    Amazon.Lambda.Core.LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace UserService.Functions
{
    public class AddNewUserFunction
    {
        public IConfigurationService ConfigService { get; }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IEnvironmentService, EnvironmentService>();
            serviceCollection.AddTransient<IConfigurationService, ConfigurationService>();
        }

        public AddNewUserFunction()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            ConfigService = serviceProvider.GetService<IConfigurationService>();
        }

        public async Task<APIGatewayHttpApiV2ProxyResponse> Handle(APIGatewayHttpApiV2ProxyRequest request,
            ILambdaContext context)
        {
            await Task.CompletedTask;

            var env2Value = ConfigService.GetConfiguration()["env2"];
            var env3Value = ConfigService.GetConfiguration()["env3"];
            LambdaLogger.Log("env2: " + env2Value);
            LambdaLogger.Log("env3: " + env3Value);

            var userRequest = JsonSerializer.Deserialize<UserRequest>(request.Body);

            var user = new User(userRequest.FirstName, userRequest.LastName);

            var response = new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = (int) HttpStatusCode.Created,
                Body = JsonSerializer.Serialize(user),
                Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}}
            };
            //todo: Add location Header

            //todo: Template Method and do these logs asynchronously
            LambdaLogger.Log(
                "ENVIRONMENT VARIABLES: " + JsonSerializer.Serialize(Environment.GetEnvironmentVariables()));
            LambdaLogger.Log("CONTEXT: " + JsonSerializer.Serialize(context.GetMainProperties()));
            LambdaLogger.Log("EVENT: " + JsonSerializer.Serialize(request.GetMainProperties()));

            return response;
        }
    }
}