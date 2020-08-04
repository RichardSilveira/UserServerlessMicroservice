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
    public class DeleteUserFunction : FunctionBase
    {
        private IConfiguration _configuration;
        private IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;

        private void Configure()
        {
            LambdaLogger.Log("Configure Starts");
            LambdaLogger.Log("_unitOfWork:" + (_unitOfWork == null).ToString());
            LambdaLogger.Log("_userRepository:" + (_userRepository == null).ToString());
            _configuration = new ConfigurationService().GetConfiguration();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            _unitOfWork = serviceProvider.GetService<IUnitOfWork>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
            LambdaLogger.Log("Configure after injection");
            LambdaLogger.Log("_unitOfWork:" + (_unitOfWork == null).ToString());
            LambdaLogger.Log("_userRepository:" + (_userRepository == null).ToString());
        }

        public DeleteUserFunction()
        {
            // Parameterless constructor required by AWS Lambda runtime 
        }

        /* You need pass all your abstractions here to have them injected for tests.
         This way neither the ConfigureServices nor the Configure won't be called.
         */
        public DeleteUserFunction(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository)
        {
            RunningAsLocal = true;

            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }


        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            var connString = _configuration["UserServiceDbContextConnectionString"];

            serviceCollection.AddDbContext<UserServiceDbContext>(options => options.UseMySql(connString));

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<SomeUserDomainService>();
        }

        public async Task<APIGatewayHttpApiV2ProxyResponse> Handle(APIGatewayHttpApiV2ProxyRequest request,
            ILambdaContext context)
        {
            //todo: bad request
            LambdaLogger.Log($"CONTEXT {Serialize(context.GetMainProperties())}");
            LambdaLogger.Log($"EVENT: {Serialize(request.GetMainProperties())}");
            if (!RunningAsLocal)
                Configure();

            LambdaLogger.Log("Path " + request.PathParameters["userid"]);
            var userId = Guid.Parse(request.PathParameters["userid"]);


            var user = await _userRepository.GetById(Guid.Parse(request.PathParameters["userid"]));
            if (user == null) return NotFound();

            _userRepository.Delete(user);
            _unitOfWork.SaveChanges(); //todo: dispose required because of how Lambda works

            var response = new APIGatewayHttpApiV2ProxyResponse
            {
                StatusCode = (int) HttpStatusCode.NoContent,
                Headers = new Dictionary<string, string> {{"Content-Type", "application/json"}}
            };

            return response;
        }
    }
}