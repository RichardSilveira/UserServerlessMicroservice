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
        public void AddUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public User GetUser() => new User("Lorem", "Ipsum");

        public void Add(User entity)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
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

        public Task<IEnumerable<User>> Get(Expression<Func<User, bool>> @where)
        {
            throw new NotImplementedException();
        }
    }
}