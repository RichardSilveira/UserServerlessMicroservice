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

        public Guid UserId { get; private set; }

        public Order(Guid userId, OrderStatus status)
        {
            this.UserId = userId;
            this.Status = status;
        }
    }
}