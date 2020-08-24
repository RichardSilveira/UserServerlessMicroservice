using System;
using MediatR;

namespace UserService.Domain.Events
{
    public class UserRegisteredDomainEvent : INotification
    {
        public Guid UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }

        public UserRegisteredDomainEvent(Guid userId, string firstName, string lastName, string email)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}