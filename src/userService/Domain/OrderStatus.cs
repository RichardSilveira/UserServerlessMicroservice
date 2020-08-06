namespace UserService.Domain
{
    /// <summary>
    /// All the relevant Order Status for the context of the User Bounded Context 
    /// </summary>
    public enum OrderStatus
    {
        Active,
        InShipping,
        Delivered,
        Cancelled
    }
}