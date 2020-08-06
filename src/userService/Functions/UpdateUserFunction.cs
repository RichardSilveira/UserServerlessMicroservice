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
using UserService.Domain.Requests;
using UserService.Domain.Validators;
using UserService.Extensions;
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

            var userReq = Deserialize<UpdateUserRequest>(request.Body);

            var userValidator = new Lazy<UpdateUserRequestValidator>(() => new UpdateUserRequestValidator());

            var userId = request.PathParameters["userid"];

            if (userReq.Id != userId)
                return BadRequest("The user ID must be the same at both body request and url path parameter");

            var userValidationResult = userValidator.Value.Validate(userReq);

            if (!userValidationResult.IsValid)
                return BadRequest(userValidationResult.Errors.ToModelFailures());

            var user = await _userRepository.GetById(Guid.Parse(userId));
            if (user == null) return NotFound();

            user.UpdatePersonalInfo(userReq.FirstName, userReq.LastName);

            if (userReq.Address != null)
            {
                var userAddressReq = userReq.Address;

                var userAddress = new Address(userAddressReq.Country, userAddressReq.Street, userAddressReq.City, userAddressReq.State);

                user.UpdateAddress(userAddress);
                //todo: UpdateAddress may raise an event (I may need to have an AddAdress as well) (checking internally)
            }
            else
                user.RemoveAddress();
            //todo: me way have a shipping associated with the user, then his address can't be removed. (domain service)
            // if denied by domain service, return badrequest and dont dispatch all list of events.

            _userRepository.Update(user);
            _unitOfWork.SaveChanges();
            _unitOfWork.Dispose();

            return Ok(user);
        }
    }
}