namespace UserService.Domain
{
    public class SomeUserDomainService
    {
        private readonly IUserRepository _userRepository;

        public SomeUserDomainService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool DoSomeLogicInvolvingUser() => true;
    }
}