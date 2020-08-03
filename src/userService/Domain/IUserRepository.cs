using System;
using System.Threading.Tasks;
using UserService.SharedKernel;

namespace UserService.Domain
{
    public interface IUserRepository: IRepository<User, Guid>
    {
    }
}