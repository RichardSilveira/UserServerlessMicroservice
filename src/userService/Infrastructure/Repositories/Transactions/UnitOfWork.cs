using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using UserService.SharedKernel;

namespace UserService.Infrastructure.Repositories.Transactions
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UserContext _context;
        private readonly IMediator _mediator;

        public UnitOfWork(UserContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            /* There is no need to always use Async every time in DB operations, but it's an optioned topic, you need to know your database workload,
            / capacity and more... */
            _context.SaveChanges();

            var domainEvents = _context.ChangeTracker.Entries<Entity>()
                .Where(it => it.Entity.DomainEvents.Any())
                .SelectMany(it => it.Entity.DomainEvents)
                .ToList();

            var publishEventTasks = new List<Task>();
            foreach (var domainEvent in domainEvents)
                publishEventTasks.Add(_mediator.Publish(domainEvent));

            await Task.WhenAll(publishEventTasks);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}