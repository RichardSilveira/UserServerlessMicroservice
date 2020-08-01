using Microsoft.Extensions.Configuration;

namespace UserService.Configuration
{
    public interface IConfigurationService
    {
        IConfiguration GetConfiguration();
    }
}