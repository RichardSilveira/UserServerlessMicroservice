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
    public class GetUserByIdFunction
    {
        private IConfiguration _configuration;
        private IUserRepository _userRepository;

        private void Configure()
        {
            _configuration = new ConfigurationService().GetConfiguration();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            _userRepository = serviceProvider.GetService<IUserRepository>();
        }

        // Invoked by AWS Lambda at runtime
        public GetUserByIdFunction()
        {
        }

        /* You need pass all your abstractions here to have them injected for tests.
         This way neither the ConfigureServices nor the Configure won't be called.
         */
        public GetUserByIdFunction(
            IConfiguration configuration,
            IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }


        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            var connString = _configuration["UserServiceDbContextConnectionString"];

            serviceCollection.AddDbContext<UserServiceDbContext>(options => options.UseMySql(connString));

            serviceCollection.AddScoped<IUserRepository, UserRepository>();
        }

        public async Task<APIGatewayHttpApiV2ProxyResponse> Handle(APIGatewayHttpApiV2ProxyRequest request,
            ILambdaContext context)
        {
            //todo: bad request
            LambdaLogger.Log($"CONTEXT {Serialize(context.GetMainProperties())}");
            LambdaLogger.Log($"EVENT: {Serialize(request.GetMainProperties())}");
            Configure();

            var userId = Guid.Parse(request.PathParameters["userid"]);

            // Could apply some logic here to approve the user deletion

            var user = await _userRepository.GetById(userId);


            var response = new APIGatewayHttpApiV2ProxyResponse()
                {Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}}};

            if (user != null)
            {
                response.Body = Serialize(user);
                response.StatusCode = (int) HttpStatusCode.OK;

                // Publish to a topic about the user search
            }
            else
            {
                response.StatusCode = (int) HttpStatusCode.NotFound;
            }

            return response;
        }
    }
}