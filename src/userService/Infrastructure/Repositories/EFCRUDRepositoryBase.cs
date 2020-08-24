using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.SharedKernel;

namespace UserService.Infrastructure.Repositories
{
    public abstract class EFCRUDRepositoryBase<TEntity, TId>
        where TEntity : Entity
        where TId : struct
    {
        protected readonly DbContext _context;

        public EFCRUDRepositoryBase(DbContext context) => _context = context;

        public virtual void Add(TEntity entity)
        {
            _context.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            // Be careful about the differences between Update, Attach, and Setting EntityState.
            // https://www.learnentityframeworkcore.com/dbcontext/modifying-data
            _context.Update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            _context.Remove(entity);
        }

        public virtual async Task<TEntity> GetByIdAsync(TId Id)
        {
            return await _context.Set<TEntity>().FindAsync(Id);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> @where, int? skip, int? take,
            Expression<Func<TEntity, object>> orderBy, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _context.Set<TEntity>().AsNoTracking().Where(where);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (orderBy != null)
                query = query.OrderBy(orderBy);

            if (includeProperties != null)
            {
                foreach (var property in includeProperties)
                {
                    query = query.Include(property);
                }
            }

            return await query.ToListAsync();
        }
    }
}