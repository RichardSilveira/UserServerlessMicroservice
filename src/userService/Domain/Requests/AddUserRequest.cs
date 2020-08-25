namespace UserService.Domain.Requests
{
    public partial class AddUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public AddressRequest Address { get; set; }
    }

    // Factories for tests only
    public partial class AddUserRequest
    {
        public static class Factory
        {
            public static AddUserRequest ValidUserSample() => new AddUserRequest()
            {
                FirstName = "Julia",
                LastName = "Doe",
                Email = "newvalidemail@email.com",
                Address = new AddressRequest()
                {
                    Country = "Brazil",
                    Street = "Flower St."
                }
            };
        }
    }
}