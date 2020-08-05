using System;
using System.Collections.Generic;
using UserService.Functions;
using UserService.SharedKernel;

namespace UserService.Domain
{
    public class User : Entity
    {
        //todo: Add email and handle with uniqueness issue
        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public Address Address { get; private set; }

        public User(string firstName, string lastName)
        {
        }

        public void UpdateAddress(Address address) => Address = address;
        public void RemoveAddress() => Address = null;

        public void UpdatePersonalInfo(string firstName, string lastName)
        {
            //todo: improve this design later 
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        }
    }
}