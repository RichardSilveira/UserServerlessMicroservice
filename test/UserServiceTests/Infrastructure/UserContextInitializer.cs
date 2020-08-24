using System;
using System.Collections.Generic;
using UserService.Domain;
using UserService.Infrastructure.Repositories;

namespace UserServiceTests.Infrastructure
{
    public static class UserContextInitializer
    {
        public static (List<User> Users, List<Order> Orders) SeedDatabase(
            UserContext context,
            bool insertDefaults = true,
            Action<List<User>, List<Order>> options = null)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var defaultUsers = new List<User>();
            var defaultOrders = new List<Order>();

            if (insertDefaults)
            {
                #region Default Seed

                var aValidAddress = new Address("Brazil");
                var userRichard = new User("Richard", "Lee", "richardleecba@gmail.com");
                userRichard.UpdateAddress(aValidAddress);

                var userJohn = new User("John", "Doe", "jonas@gmail.com");
                defaultUsers.AddRange(new List<User>()
                {
                    userRichard,
                    userJohn
                });

                var activeOrderForRichard = new Order(userRichard.Id, OrderStatus.Active);
                var inShippingOrderForJohn = new Order(userJohn.Id, OrderStatus.InShipping);

                defaultOrders.AddRange(new List<Order>()
                {
                    activeOrderForRichard,
                    inShippingOrderForJohn
                });

                #endregion
            }

            options?.Invoke(defaultUsers, defaultOrders);

            context.Users.AddRange(defaultUsers);
            context.Orders.AddRange(defaultOrders);

            context.SaveChanges();

            return (defaultUsers, defaultOrders);
        }

        public static void ClearDatabase(UserContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}