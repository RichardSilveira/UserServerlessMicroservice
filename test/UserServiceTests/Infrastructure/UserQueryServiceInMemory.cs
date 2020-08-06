using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Domain;

namespace UserServiceTests.Infrastructure
{
    public class UserQueryServiceInMemory : IUserQueryService
    {
        public Task<IEnumerable<User>> GetUsersByEmail(string email, Guid ignoredId)
        {
            throw new NotImplementedException();
        }
    }
}