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

        public void Delete(User entity)
        {
            //TODO: Implemen it later
        }

        public Task<User> GetByIdAsync(Guid Id)
        {
            return Task.FromResult<User>(new User("Lorem", "Ipsum", "email@email.com"));
        }
    }
}