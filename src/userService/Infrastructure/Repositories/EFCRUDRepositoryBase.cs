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

        public virtual async Task<TEntity> GetById(TId Id)
        {
            return await _context.Set<TEntity>().FindAsync(Id);
        }
    }
}