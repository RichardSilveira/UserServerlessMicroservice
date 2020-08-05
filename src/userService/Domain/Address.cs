using System;
using System.Collections.Generic;
using UserService.SharedKernel;

namespace UserService.Domain
{
    public class Address : ValueObject
    {
        public string Country { get; private set; }
        public string State { get; private set; }
        public string City { get; private set; }
        public string Street { get; private set; }

        public Address()
        {
            //For EF
        }

        public Address(string country, string street = null, string city = null, string state = null)
        {
            Country = country;
            State = state;
            City = city;
            Street = street;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // return an element at time because the verification can be cancelled as soon as possible
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
        }
    }
}