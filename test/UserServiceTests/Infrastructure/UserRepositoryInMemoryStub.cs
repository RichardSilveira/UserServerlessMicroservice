using UserService.Domain;
using UserService.Functions;

namespace UserServiceTests.Infrastructure
{
    public class UserRepositoryInMemoryStub : IUserRepository
    {
        public User GetUser() => new User("Lorem", "Ipsum");
    }
}