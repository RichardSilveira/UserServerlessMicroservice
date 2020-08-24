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

namespace UserService.Functions
{
    public class UpdateUserFunction : FunctionBase
    {
        private IUnitOfWork _unitOfWork;
        private IUserRepository _userRepository;
        private UserOrderDomainService _userOrderService;

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            var connString = Configuration["UserServiceDbContextConnectionString"];

            serviceCollection.AddDbContext<UserContext>(options => options.UseMySql(connString));

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IUserQueryService, UserQueryService>();
        }

        protected override void Configure(IServiceProvider serviceProvider)
        {
            _unitOfWork = serviceProvider.GetService<IUnitOfWork>();
            _userRepository = serviceProvider.GetService<IUserRepository>();
            _userOrderService = serviceProvider.GetService<UserOrderDomainService>();
        }

        // Parameterless constructor required by AWS Lambda runtime 
        public UpdateUserFunction()
        {
        }


        public UpdateUserFunction(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            UserOrderDomainService userOrderService) : base(configuration)
        {
            // Constructor used by tests
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userOrderService = userOrderService;
            _userOrderService = userOrderService;
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

            var user = await _userRepository.GetByIdAsync(Guid.Parse(userId));
            if (user == null) return NotFound();

            user.UpdatePersonalInfo(userReq.FirstName, userReq.LastName);

            Address userAddress = null;
            if (userReq.Address != null)
            {
                var userAddressReq = userReq.Address;
                userAddress = new Address(userAddressReq.Country, userAddressReq.Street, userAddressReq.City, userAddressReq.State);
            }

            var result = await _userOrderService.CanUpdateAddressFromExistingUser(user);
            if (!result.IsValid)
                return BadRequest(result.ReasonPhrase);

            _userOrderService.UpdateAddressFromExistingUser(user, userAddress);

            _userRepository.Update(user);
            _unitOfWork.SaveChanges();
            _unitOfWork.Dispose();

            return Ok(user);
        }
    }
}