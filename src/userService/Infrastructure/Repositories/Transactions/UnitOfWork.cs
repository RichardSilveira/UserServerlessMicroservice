using System;
using System.Threading.Tasks;

namespace UserService.Infrastructure.Repositories.Transactions
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserServiceDbContext _context;

        public UnitOfWork(UserServiceDbContext context)
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