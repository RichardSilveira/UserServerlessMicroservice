using System.IO;
using Microsoft.Extensions.Configuration;
using UserService.Configuration;

namespace UserService
{
    public class ConfigurationService : IConfigurationService
    {
        public IEnvironmentService EnvService { get; }
        private readonly IConfiguration _configuration;

        public ConfigurationService(IEnvironmentService envService)
        {
            EnvService = envService;
            _configuration = BuildConfiguration();
        }

        public IConfiguration GetConfiguration() => _configuration;

        private IConfiguration BuildConfiguration() => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{EnvService.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }
}