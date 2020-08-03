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
    public class UserRepository : RepositoryBase<User, Guid>, IUserRepository
    {
        //todo: Flavor composition over inheritance here (SOLID - SL)
        private readonly DbContext _context;


        public UserRepository(DbContext context) : base(context) => _context = context;

        public void Add(User entity)
        {
            base.Add(entity);
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid Id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetById(Guid Id)
        {
            return await base.GetById(Id);
        }

        public Task<IEnumerable<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> Get(Expression<Predicate<User>> @where)
        {
            throw new NotImplementedException();
        }
    }
}