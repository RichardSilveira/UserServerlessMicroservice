using System;
using System.Collections.Generic;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Extensions;
using static System.Text.Json.JsonSerializer;

namespace UserService.Functions
{
    public abstract class FunctionBase
    {
        protected IConfiguration Configuration { get; private set; }
        protected bool RunningAsLocal = false;

        public FunctionBase() => Configuration = ConfigurationService.Instance.Configuration;

        public FunctionBase(IConfiguration configuration)
        {
            Configuration = configuration;
            RunningAsLocal = true;
        }

        protected void ConfigureDependencies()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Configure(serviceCollection.BuildServiceProvider());
        }

        protected abstract void ConfigureServices(IServiceCollection serviceCollection);
        protected abstract void Configure(IServiceProvider serviceProvider);

        protected void LogFunctionMetadata(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
        {
            LambdaLogger.Log($"CONTEXT {Serialize(context.GetMainProperties())}");
            LambdaLogger.Log($"EVENT: {Serialize(request.GetMainProperties())}");
        }

        protected APIGatewayHttpApiV2ProxyResponse Ok() =>
            new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.OK,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"}
                }
            };

        protected APIGatewayHttpApiV2ProxyResponse Ok(object body) =>
            new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.OK,
                Body = Serialize(body),
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"}
                }
            };

        protected APIGatewayHttpApiV2ProxyResponse Ok(string body) =>
            new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.OK,
                Body = body,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"}
                }
            };

        protected APIGatewayHttpApiV2ProxyResponse Ok(object body, Action<APIGatewayHttpApiV2ProxyResponse> options)
        {
            var response = new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.OK,
                Body = Serialize(body)
            };

            options(response);

            return response;
        }

        protected APIGatewayHttpApiV2ProxyResponse Created(object body) =>
            new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.Created,
                Body = Serialize(body),
                Headers = new Dictionary<string, string>
                {
                    {
                        "Content-Type", "application/json"
                    }
                }
            };


        protected APIGatewayHttpApiV2ProxyResponse Created(string body) =>
            new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.Created,
                Body = body,
                Headers = new Dictionary<string, string>
                {
                    {
                        "Content-Type", "application/json"
                    }
                }
            };

        protected APIGatewayHttpApiV2ProxyResponse Created(object body, Action<APIGatewayHttpApiV2ProxyResponse> options)
        {
            var response = new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.Created,
                Body = Serialize(body)
            };

            options(response);

            return response;
        }

        protected APIGatewayHttpApiV2ProxyResponse NoContent() =>
            new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.NoContent,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"}
                }
            };

        protected APIGatewayHttpApiV2ProxyResponse NotFound() =>
            new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.NotFound,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"}
                }
            };

        protected APIGatewayHttpApiV2ProxyResponse BadRequest(string errorMessage) =>
            new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.BadRequest,
                Body = errorMessage,
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"}
                }
            };

        protected APIGatewayHttpApiV2ProxyResponse BadRequest(IEnumerable<ModelFailure> errors) =>
            new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.BadRequest,
                Body = Serialize(errors),
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"}
                }
            };
        
        protected APIGatewayHttpApiV2ProxyResponse BadRequest(ModelFailure error) =>
            new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.BadRequest,
                Body = Serialize(error),
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"}
                }
            };
    }
}