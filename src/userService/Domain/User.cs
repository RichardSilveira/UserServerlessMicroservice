using System;
using System.Collections.Generic;
using UserService.Functions;
using UserService.SharedKernel;

namespace UserService.Domain
{
    public class User : Entity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public Address Address { get; protected set; }

        public User(string firstName, string lastName)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        }

        public void SetAddress(Address address) => Address = address;
    }
}