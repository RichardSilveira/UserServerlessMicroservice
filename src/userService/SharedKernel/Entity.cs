using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using MediatR;

namespace UserService.SharedKernel
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }

        private readonly List<INotification> _domainEvents;
        [JsonIgnore] public ReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid(); //TODO: Use a Flake ID implementation later
            _domainEvents = new List<INotification>();
        }

        public void AddDomainEvent(INotification eventItem) => _domainEvents.Add(eventItem);

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}