using System;
using System.Threading.Tasks;

namespace UserService.Infrastructure.Repositories.Transactions
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();

        Task SaveChangesAsync();
    }
}