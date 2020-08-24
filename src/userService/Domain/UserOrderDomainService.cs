using System;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Domain
{
    public class UserOrderDomainService
    {
        private readonly IOrderQueryService _orderQueryService;

        public UserOrderDomainService(IOrderQueryService orderQueryService)
        {
            _orderQueryService = orderQueryService;
        }

        public async Task<(bool IsValid, string ReasonPhrase)> CanUpdateAddressFromExistingUser(User user)
        {
            var inShippingOrders = await _orderQueryService.GetInShippingOrdersByUserAsync(user.Id);

            if (inShippingOrders.Any())
            {
                return (false, "The address can't be changed because there is a shipping in progress. Contact the support team please.");
            }

            return (true, "");
        }

        public void UpdateAddressFromExistingUser(User user, Address newAddress)
        {
            if (newAddress != null)
                user.UpdateAddress(newAddress);
            else
                user.RemoveAddress();
        }
    }
}