namespace UserService.Domain.Requests
{
    public class AddressRequest
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}