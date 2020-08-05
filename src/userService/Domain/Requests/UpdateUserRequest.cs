namespace UserService.Domain.Requests
{
    public class UpdateUserRequest
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressRequest Address { get; set; }
    }
}