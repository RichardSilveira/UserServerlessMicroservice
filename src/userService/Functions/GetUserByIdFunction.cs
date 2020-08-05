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

            serviceCollection.AddDbContext<UserServiceDbContext>(options => options.UseMySql(connString));

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

        /* You need pass all your abstractions here to have them injected for tests.
         This way neither the ConfigureServices nor the Configure won't be called.
         */
        public GetUserByIdFunction(
            IConfiguration configuration,
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<APIGatewayHttpApiV2ProxyResponse> Handle(APIGatewayHttpApiV2ProxyRequest request,
            ILambdaContext context)
        {
            //todo: bad request
            LambdaLogger.Log($"CONTEXT {Serialize(context.GetMainProperties())}");
            LambdaLogger.Log($"EVENT: {Serialize(request.GetMainProperties())}");

            if (!RunningAsLocal)
                ConfigureDependencies();

            var userId = Guid.Parse(request.PathParameters["userid"]);

            // Could apply some logic here to approve the user deletion

            var user = await _userRepository.GetById(userId);
            if (user == null) return NotFound();

            return Ok(user);
        }
    }
}