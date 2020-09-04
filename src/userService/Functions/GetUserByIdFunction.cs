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

namespace UserService.Functions
{
    public class GetUserByIdFunction : FunctionBase
    {
        private IUserRepository _userRepository;

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var connString = Configuration["UserServiceDbContextConnectionString"];

            // serviceCollection.AddDbContext<UserContext>(options => options.UseMySql(connString));
            serviceCollection.AddDbContext<UserContext>(options => options.UseInMemoryDatabase(connString)); //temporarily

            serviceCollection.AddScoped<IUserRepository, UserRepository>();
        }

        protected override void Configure(IServiceProvider serviceProvider)
        {
            _userRepository = serviceProvider.GetService<IUserRepository>();
        }

        // Invoked by AWS Lambda at runtime
        public GetUserByIdFunction()
        {
        }

        public GetUserByIdFunction(
            IConfiguration configuration,
            IUserRepository userRepository)
        {
            // Constructor used by tests
            _userRepository = userRepository;
        }

        public APIGatewayProxyResponse HandleV2(APIGatewayProxyRequest request, ILambdaContext context)
        {
            LambdaLogger.Log("Alternative version using APIGatewayProxyResponse class instead");
            var user = new User("Richard", "Lee", "richardleecba@gmail.com");

            var response = new APIGatewayProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.OK,
                Body = Serialize(user),
                Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"},
                    {"Access-Control-Allow-Origin", "*"},
                    {"Access-Control-Allow-Credentials", "true"}
                }
            };

            return response;
        }
        

        public APIGatewayHttpApiV2ProxyResponse HandleV7(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
        {
            LambdaLogger.Log("Original version 7 using APIGatewayHttpApiV2ProxyResponse class");
            var user = new User("Richard", "Lee", "richardleecba@gmail.com");

            return Ok(user);
            
            var  headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            headers.Add("Access-Control-Allow-Origin", "*");
            headers.Add("Access-Control-Allow-Credentials", "true");
            
            var response = new APIGatewayHttpApiV2ProxyResponse()
            {
                StatusCode = (int) HttpStatusCode.OK,
                Body = Serialize(user),
                Headers = headers
            };

            return response;
        }

        public async Task<APIGatewayHttpApiV2ProxyResponse> Handle(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
        {
            LambdaLogger.Log("XXX using APIGatewayHttpApiV2ProxyResponse class");
            LogFunctionMetadata(request, context);

            if (!RunningAsLocal) ConfigureDependencies();

            var userId = Guid.Parse(request.PathParameters["userid"]);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return NotFound();

            return Ok(user);
        }
    }
}