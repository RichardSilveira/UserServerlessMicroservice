namespace UserService.Domain
{
    public class User
    {
        //todo: id as uuid (flake)
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public CivilAddress Address { get; private set; } //todo: may have a collection of addresses

        public User()
        {
        }
    }
}