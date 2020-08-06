using System;
using System.Collections.Generic;
using System.Linq;
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
using UserService.Domain.Requests;
using UserService.Domain.Validators;
using UserService.Extensions;
using UserService.Infrastructure.Repositories;
using UserService.Infrastructure.Repositories.Transactions;
using static System.Text.Json.JsonSerializer;

// If targeting .NET Core 3.1 this serializer is highly recommend over Amazon.Lambda.Serialization.Json and can significantly reduce cold start performance in Lambda.
[assembly:
    Amazon.Lambda.Core.LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace UserService.Functions
{
    public class AddNewUserFunction : FunctionBase
    {
        private IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        private IUserQueryService _userQueryService;

        private SomeUserDomainService _userDomainService;


        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var connString = Configuration["UserServiceDbContextConnectionString"];

            serviceCollection.AddDbContext<UserServiceDbContext>(options => options.UseMySql(connString));

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IUserQueryService, UserQueryService>();
            serviceCollection.AddScoped<SomeUserDomainService>();
        }

        protected override void Configure(IServiceProvider serviceProvider)
        {
            _unitOfWork = serviceProvider.GetService<IUnitOfWork>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _userDomainService = serviceProvider.GetService<SomeUserDomainService>();
            _userQueryService = serviceProvider.GetService<IUserQueryService>();
        }

        // Parameterless constructor required by AWS Lambda runtime 
        public AddNewUserFunction()
        {
        }

        public AddNewUserFunction(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            SomeUserDomainService userDomainService) : base(configuration)
        {
            //todo: _userQueryService
            // Constructor used by tests
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userDomainService = userDomainService;
        }

        public async Task<APIGatewayHttpApiV2ProxyResponse> Handle(APIGatewayHttpApiV2ProxyRequest request, ILambdaContext context)
        {
            LogFunctionMetadata(request, context);

            if (!RunningAsLocal) ConfigureDependencies();

            var userReq = Deserialize<AddUserRequest>(request.Body);

            var userValidator = new AddUserRequestValidator();
            var userValidationResult = userValidator.Validate(userReq);

            if (!userValidationResult.IsValid)
                return BadRequest(userValidationResult.Errors.ToModelFailures());


            var usersByEmail = await _userQueryService.GetUsersByEmail(userReq.Email);

            if (!usersByEmail.Any())
                return BadRequest(ModelFailure.BuildModelFailure<User>(p => p.Email, "The E-mail already exists."));

            var user = new User(userReq.FirstName, userReq.LastName, userReq.Email);

            if (userReq.Address != null)
            {
                var userAddressReq = userReq.Address;

                var userAddress = new Address(userAddressReq.Country, userAddressReq.Street, userAddressReq.City,
                    userAddressReq.State);

                user.UpdateAddress(userAddress); //todo: UpdateAddress may raise an event (I may need to have an AddAdress as well)
            }

            _userRepository.Add(user);
            _unitOfWork.SaveChanges();
            _unitOfWork.Dispose(); // Sounds good dispose explicitly because of the Lambda "nature"


            return Created(user, options =>
            {
                options.Headers = new Dictionary<string, string>
                {
                    {"Content-Type", "application/json"},
                    {"Location", $"{Configuration["apiRootUri"]}/{Configuration["STAGE"]}/v1/users/{user.Id}"}
                };
            });
        }
    }
}