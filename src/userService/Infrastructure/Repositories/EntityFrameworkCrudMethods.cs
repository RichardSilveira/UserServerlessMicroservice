using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.SharedKernel;

namespace UserService.Infrastructure.Repositories
{
    public class EntityFrameworkCrudMethods<TEntity, TId>
        where TEntity : Entity
        where TId : struct
    {
        private readonly DbContext _context;

        public EntityFrameworkCrudMethods(DbContext context) => _context = context;

        public void Add(TEntity entity)
        {
            _context.Add(entity);
        }

        public void Update(TEntity entity)
        {
            // Be careful about the differences between Update, Attach, and Setting EntityState.
            // https://www.learnentityframeworkcore.com/dbcontext/modifying-data
            _context.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public async Task<TEntity> GetById(TId Id)
        {
            return await _context.Set<TEntity>().FindAsync(Id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> where)
        {
            return await _context.Set<TEntity>().AsNoTracking().Where(where).ToListAsync();
        }
    }
}