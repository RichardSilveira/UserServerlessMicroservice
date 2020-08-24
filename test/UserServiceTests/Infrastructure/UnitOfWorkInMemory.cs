using System.Threading.Tasks;
using UserService.Infrastructure.Repositories.Transactions;

namespace UserServiceTests.Infrastructure
{
    public class UnitOfWorkInMemory : IUnitOfWork
    {
        public void SaveChanges()
        {
        }

        public Task SaveChangesAsync()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}