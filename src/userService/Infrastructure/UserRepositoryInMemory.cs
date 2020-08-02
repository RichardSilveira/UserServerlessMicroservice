using UserService.Domain;

namespace UserService.Functions
{
    public class UserRepositoryInMemory : IUserRepository
    {
        public User GetUser() => new User("Foo", "Bar");
    }
}