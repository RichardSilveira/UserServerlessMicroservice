using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Configuration;

namespace UserService.Functions
{
    public abstract class FunctionBase
    {
        public IConfiguration Configuration { get; private set; }

        public FunctionBase()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            Configure(serviceProvider);
        }

        public FunctionBase(IConfiguration configuration) => Configuration = configuration;

        protected abstract void ConfigureServices(IServiceCollection serviceCollection);

        protected virtual void Configure(IServiceProvider serviceProvider)
        {
            Configuration = serviceProvider.GetService<IConfigurationService>().GetConfiguration();
        }
    }
}