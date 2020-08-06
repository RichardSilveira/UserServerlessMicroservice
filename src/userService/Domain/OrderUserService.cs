namespace UserService.Domain
{
    public class OrderUserService
    {
        private readonly IUserQueryService _userQueryService;

        public OrderUserService(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
        }
        
        
    }
}