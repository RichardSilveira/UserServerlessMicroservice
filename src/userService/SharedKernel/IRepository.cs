namespace UserService.SharedKernel
{
    public interface IRepository<T> where T : IAggregateRoot
    {
    }
}