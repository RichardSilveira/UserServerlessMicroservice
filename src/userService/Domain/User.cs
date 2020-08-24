﻿using System;
using System.Collections.Generic;
using UserService.Domain.Events;
using UserService.Functions;
using UserService.SharedKernel;

namespace UserService.Domain
{
    public class User : Entity, IAggregateRoot
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }

        public Address Address { get; private set; }

        public User(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;

            AddDomainEvent(new UserRegisteredDomainEvent(Id, FirstName, LastName, Email));
        }


        public void UpdateAddress(Address address) => Address = address;
        public void RemoveAddress() => Address = null;

        public void UpdatePersonalInfo(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            // We don't allow email update at this sample
        }
    }
}