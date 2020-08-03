using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UserService.SharedKernel
{
    public interface IRepository<TEntity, TId>
        where TEntity : Entity
        where TId : struct
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        Task Delete(TId Id);

        Task<TEntity> GetById(TId Id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> where);
    }
}