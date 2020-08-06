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

        public async Task<IEnumerable<User>> GetUsersByEmail(string email)
        {
            var users = from u in _context.Users.AsNoTracking()
                where u.Email == email
                select u;

            return await users.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetInShippingOrdersToUser(Guid userId)
        {
            var orders = from o in _context.Orders
                where o.UserId == userId && o.Status == OrderStatus.InShipping
                orderby o.OrderDate
                select o;

            return await orders.ToListAsync();
        }
    }
}