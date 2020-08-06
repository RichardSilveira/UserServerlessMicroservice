using System;
using UserService.SharedKernel;

namespace UserService.Domain
{
    /// <summary>
    /// Order for the context of the User Bounded Context.
    /// </summary>
    public class Order : Entity
    {
        public OrderStatus Status { get; private set; }

        public DateTimeOffset OrderDate { get; private set; }

        public Guid UserId { get; private set; }
    }
}