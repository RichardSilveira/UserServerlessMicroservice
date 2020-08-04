using System;
using UserService.Configuration;

namespace UserService.Extensions
{
    public static class EnvironmentExtensions
    {
        public static bool IsLocalTest() => IsEnvironment("test");
        public static bool IsDevelopment() => IsEnvironment("dev");
        public static bool IsProduction() => IsEnvironment("prod");

        private static bool IsEnvironment(string environment) =>
            string.Equals(new EnvironmentService().EnvironmentName, environment,
                StringComparison.CurrentCultureIgnoreCase);
    }
}