using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Domain;

namespace UserService.Infrastructure.Repositories
{
    public class OrderQueryService : IOrderQueryService
    {
        private readonly UserContext _context;

        public OrderQueryService(UserContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetInShippingOrdersByUserAsync(Guid userId)
        {
            var orders = from o in _context.Orders.AsNoTracking()
                where o.UserId == userId && o.Status == OrderStatus.InShipping
                select o;

            return await orders.ToListAsync();
        }
    }
}