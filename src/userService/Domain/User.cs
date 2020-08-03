using System;
using UserService.Functions;
using UserService.SharedKernel;

namespace UserService.Domain
{
    public class User: Entity
    {
        //todo: id as uuid (flake)
        public string FirstName { get; private set; }

        public string LastName { get; private set; }
        //TODO: public CivilAddress Address { get; private set; } //todo: may have a collection of addresses

        public User(string firstName, string lastName)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));

            //todo: handle with domain events after commiting to the database later (syncronous with mediatR?)
        }
    }
}