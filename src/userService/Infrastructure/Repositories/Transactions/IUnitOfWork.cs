using System.Threading.Tasks;

namespace UserService.Infrastructure.Repositories.Transactions
{
    public interface IUnitOfWork
    {
        void SaveChanges();
    }
}