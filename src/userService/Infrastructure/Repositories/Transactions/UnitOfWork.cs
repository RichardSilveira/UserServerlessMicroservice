using System;
using System.Threading.Tasks;

namespace UserService.Infrastructure.Repositories.Transactions
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserContext _context;

        public UnitOfWork(UserContext context)
        {
            _context = context;
        }

        public void SaveChanges() => _context.SaveChanges();

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}