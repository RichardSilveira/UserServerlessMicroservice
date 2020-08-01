using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Serialization.SystemTextJson;

// If targeting .NET Core 3.1 this serializer is highly recommend over Amazon.Lambda.Serialization.Json and can significantly reduce cold start performance in Lambda.
[assembly:
    Amazon.Lambda.Core.LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AwsDotnetCsharp
{
    public class AddNewUserHandler
    {
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
        public async Task<APIGatewayHttpApiV2ProxyResponse> Handle(APIGatewayHttpApiV2ProxyRequest request,
            ILambdaContext context)
        {
            await Task.CompletedTask;
            
            var user = new User("Richard");

            var response = new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = (int) HttpStatusCode.OK,
                Body = JsonSerializer.Serialize(user),
                Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}}
            };

            return response;
        }

        private class User
        {
            public User(string firstName)
            {
                FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            }

            public string FirstName { get; private set; }
        }
    }
}