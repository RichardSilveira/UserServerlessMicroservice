using System;
using MediatR;

namespace UserService.Domain.Events
{
    public class UserAddressAddedDomainEvent : INotification
    {
        public Guid UserId { get; }
        public string FullName { get; }
        public string Email { get; }

        public string Country { get; }
        public string State { get; }
        public string City { get; }
        public string Street { get; }

        public UserAddressAddedDomainEvent(Guid userId, string fullName, string email, string country, string state, string city, string street)
        {
            UserId = userId;
            FullName = fullName;
            Email = email;
            Country = country;
            State = state;
            City = city;
            Street = street;
        }
    }
}