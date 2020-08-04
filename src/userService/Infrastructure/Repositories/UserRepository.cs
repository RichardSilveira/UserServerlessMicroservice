using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Domain;
using UserService.Functions;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserServiceDbContext _context;
        private readonly EntityFrameworkCrudMethods<User, Guid> _crudMethods;

        public UserRepository(UserServiceDbContext context)
        {
            _context = context;
            _crudMethods = new EntityFrameworkCrudMethods<User, Guid>(_context);
        }

        public void Add(User entity)
        {
            _crudMethods.Add(entity);
        }

        public void Update(User entity)
        {
            _crudMethods.Update(entity);
        }

        public void Delete(User entity)
        {
            _crudMethods.Delete(entity);
        }

        public async Task<User> GetById(Guid Id)
        {
            return await _crudMethods.GetById(Id);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _crudMethods.GetAll();
        }
    }
}