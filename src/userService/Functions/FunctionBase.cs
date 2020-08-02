using Microsoft.Extensions.DependencyInjection;
using UserService.Configuration;

namespace UserService.Functions
{
    public abstract class FunctionBase
    {
        protected IConfigurationService ConfigService { get; }

        public FunctionBase()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            ConfigService = serviceProvider.GetService<IConfigurationService>();
        }

        public FunctionBase(IConfigurationService configService) => ConfigService = configService;

        protected abstract void ConfigureServices(IServiceCollection serviceCollection);
    }
}