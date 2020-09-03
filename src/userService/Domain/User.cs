using UserService.Domain.Events;
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

        public void AddAddress(Address address)
        {
            Address = address;

            // Because I'm working with Event Sourcing, there is no need to raise an event of all of your aggregates methods 
            // if you're not planning to work with Event Sourcing, raise events of the main ones, e.g. that will cause some effect in our application.
            AddDomainEvent(new UserAddressAddedDomainEvent(Id, $"{FirstName} {LastName}", Email,
                Address.Country, Address.State, Address.City, Address.Street));
        }


        public void UpdateAddress(Address address)
        {
            Address = address;

            AddDomainEvent(new UserAddressUpdatedDomainEvent(Id, $"{FirstName} {LastName}", Email,
                Address.Country, Address.State, Address.City, Address.Street));
        }

        public void RemoveAddress() => Address = null;//todo: Raise event

        public void UpdatePersonalInfo(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            // We don't allow email update at this sample
            
            //todo: Raise event
        }
    }
}