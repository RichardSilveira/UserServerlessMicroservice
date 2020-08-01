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
    public class HelloWorld
    {
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
        public async Task<APIGatewayHttpApiV2ProxyResponse> Handle(APIGatewayHttpApiV2ProxyRequest request,
            ILambdaContext context)
        {
            var queryStringParams = request.QueryStringParameters.ToImmutableList();


            var result = await Task.FromResult(queryStringParams);
            var user = new User();
            user.Name = "Richard";

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
            public string Name { get; set; }
        }
    }
}