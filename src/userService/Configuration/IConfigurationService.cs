using Microsoft.Extensions.Configuration;

namespace UserService
{
    public interface IConfigurationService
    {
        IConfiguration GetConfiguration();
    }
}