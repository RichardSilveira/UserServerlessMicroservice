using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UserService.Domain;
using UserService.Functions;

namespace UserServiceTests.Infrastructure
{
    public class UserRepositoryInMemory : IUserRepository
    {
        public void Add(User entity)
        {
            //TODO: Implemen it later
        }

        public void Update(User entity)
        {
            //TODO: Implemen it later
        }

        public Task Delete(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetById(Guid Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}