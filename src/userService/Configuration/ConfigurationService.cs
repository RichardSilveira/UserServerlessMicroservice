using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using UserService.Configuration;

namespace UserService
{
    public class ConfigurationService
    {
        private readonly EnvironmentService _environmentService;
        public IConfiguration Configuration { get; private set; }

        private static readonly Lazy<ConfigurationService> _instance =
            new Lazy<ConfigurationService>(() => new ConfigurationService(), true);

        public static ConfigurationService Instance => _instance.Value;

        private ConfigurationService()
        {
            _environmentService = new EnvironmentService();
            Configuration = BuildConfiguration();

            IConfiguration BuildConfiguration() => new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_environmentService.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static IConfiguration BuildConfigurationForTests(string environmentName) =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
    }
}