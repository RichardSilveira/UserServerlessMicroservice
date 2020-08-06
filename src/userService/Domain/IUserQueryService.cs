using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserService.Domain
{
    public interface IUserQueryService
    {
        public Task<IEnumerable<User>> GetUsersByEmail(string email, Guid ignoredId);
    }
}