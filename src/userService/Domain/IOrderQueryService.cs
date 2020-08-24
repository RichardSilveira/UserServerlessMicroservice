using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserService.Domain
{
    public interface IOrderQueryService
    {
        Task<IEnumerable<Order>> GetInShippingOrdersByUserAsync(Guid userId);
    }
}