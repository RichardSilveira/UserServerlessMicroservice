using UserService.Domain;

namespace UserService.Functions
{
    public interface IUserRepository
    {
        User GetUser();
    }
}