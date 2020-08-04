using System;
using System.Collections.Generic;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using static System.Text.Json.JsonSerializer;

namespace UserService.Functions
{
    public class FunctionBase
    {
        protected bool RunningAsLocal = false;

        protected APIGatewayHttpApiV2ProxyResponse Ok() => new APIGatewayHttpApiV2ProxyResponse()
        {
            StatusCode = (int) HttpStatusCode.OK,
            Headers = new Dictionary<string, string>
            {
                {"Content-Type", "application/json"}
            }
        };

        protected APIGatewayHttpApiV2ProxyResponse Ok(object body) => new APIGatewayHttpApiV2ProxyResponse()
        {
            StatusCode = (int) HttpStatusCode.OK,
            Body = Serialize(body),
            Headers = new Dictionary<string, string>
            {
                {"Content-Type", "application/json"}
            }
        };

        protected APIGatewayHttpApiV2ProxyResponse Ok(string body) => new APIGatewayHttpApiV2ProxyResponse()
        {
            StatusCode = (int) HttpStatusCode.OK,
            Body = body,
            Headers = new Dictionary<string, string>
            {
                {"Content-Type", "application/json"}
            }
        };

        protected APIGatewayHttpApiV2ProxyResponse Ok(Action<APIGatewayHttpApiV2ProxyResponse> options)
        {
            var response = new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.OK
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

        protected APIGatewayHttpApiV2ProxyResponse Created(Action<APIGatewayHttpApiV2ProxyResponse> options)
        {
            var response = new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.Created
            };

            options(response);

            return response;
        }

        protected APIGatewayHttpApiV2ProxyResponse NoContent() => new APIGatewayHttpApiV2ProxyResponse()
        {
            StatusCode = (int) HttpStatusCode.NoContent,
            Headers = new Dictionary<string, string>
            {
                {"Content-Type", "application/json"}
            }
        };
        
        protected APIGatewayHttpApiV2ProxyResponse NotFound() => new APIGatewayHttpApiV2ProxyResponse()
        {
            StatusCode = (int) HttpStatusCode.NotFound,
            Headers = new Dictionary<string, string>
            {
                {"Content-Type", "application/json"}
            }
        };
    }
}