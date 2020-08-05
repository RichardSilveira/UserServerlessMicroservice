namespace UserService.Domain.Requests
{
    public class AddUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressRequest Address { get; set; }
    }
}