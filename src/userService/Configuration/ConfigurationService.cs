using System.IO;
using Microsoft.Extensions.Configuration;
using UserService.Configuration;

namespace UserService
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IEnvironmentService _environmentService;
        private readonly IConfiguration _configuration;

        public ConfigurationService()
        {
            _environmentService = new EnvironmentService();
            _configuration = BuildConfiguration();
        }

        public IConfiguration GetConfiguration() => _configuration;

        private IConfiguration BuildConfiguration() => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{_environmentService.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}