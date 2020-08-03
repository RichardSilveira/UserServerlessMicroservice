using System.Text.Json.Serialization;

namespace UserService.Functions
{
    public class UpdateUserRequest
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        public bool HasSomeAddressInfo() => Country != "" || State != "" || City != "" || Street != "";
        //TODO: I know, but I'm really tired right now =\
    }
}