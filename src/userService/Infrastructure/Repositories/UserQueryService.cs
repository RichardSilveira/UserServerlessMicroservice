using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Domain;

namespace UserService.Infrastructure.Repositories
{
    public class UserQueryService : IUserQueryService
    {
        private readonly UserServiceDbContext _context;

        public UserQueryService(UserServiceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersByEmail(string email, Guid? ignoredId)
        {
            var query = _context.Set<User>().AsNoTracking().Where(x =>
                string.Equals(x.Email.ToUpper(), email.ToUpper(), StringComparison.InvariantCultureIgnoreCase));

            if (ignoredId.HasValue)
                query = query.Where(x => x.Id != ignoredId);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUser(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}