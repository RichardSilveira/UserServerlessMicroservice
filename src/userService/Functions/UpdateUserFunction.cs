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
    public class UpdateUserFunction : FunctionBase
    {
        private IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;

        private SomeUserDomainService _userDomainService;

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var connString = Configuration["UserServiceDbContextConnectionString"];

            serviceCollection.AddDbContext<UserServiceDbContext>(options => options.UseMySql(connString));

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<SomeUserDomainService>();
        }

        protected override void Configure(IServiceProvider serviceProvider)
        {
            _unitOfWork = serviceProvider.GetService<IUnitOfWork>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _userDomainService = serviceProvider.GetService<SomeUserDomainService>();
        }

        // Parameterless constructor required by AWS Lambda runtime 
        public UpdateUserFunction()
        {
        }


        public UpdateUserFunction(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            SomeUserDomainService userDomainService) : base(configuration)
        {
            // Constructor used by tests
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userDomainService = userDomainService;
        }

        public async Task<APIGatewayHttpApiV2ProxyResponse> Handle(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
        {
            LogFunctionMetadata(request, context);

            if (!RunningAsLocal) ConfigureDependencies();

            var userRequest = Deserialize<UpdateUserRequest>(request.Body);

            var user = await _userRepository.GetById(Guid.Parse(request.PathParameters["userid"]));
            if (user == null) return NotFound();


            user.UpdatePersonalInfo(userRequest.FirstName, userRequest.LastName);

            if (userRequest.HasSomeAddressInfo())
            {
                var address = new Address(userRequest.Country, userRequest.Street, userRequest.City,
                    userRequest.State);

                user.UpdateAddressInfo(address); //todo: raise event to signalize the own user or other service about it
            }
            else
            {
                user.RemoveAddress(); //todo: me way have a shipping associated with the user, then his address can't be removed.
            }

            _userRepository.Update(user);
            _unitOfWork.SaveChanges();
            _unitOfWork.Dispose();

            return Ok(user);
        }
    }
}