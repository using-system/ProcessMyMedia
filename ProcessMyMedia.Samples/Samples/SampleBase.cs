using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProcessMyMedia.Model;
using ProcessMyMedia.Services;
using ProcessMyMedia.Services.Contract;
using System;

namespace ProcessMyMedia.Samples
{
    public abstract class SampleBase
    {
        protected IConfigurationRoot configuration;

        public SampleBase(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public abstract void Execute();

        protected IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddMediaServices();
            services.AddLogging();

            var serviceProvider = services.BuildServiceProvider();

            //config logging
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddDebug();

            serviceProvider.UseMediaServices(new MediaConfiguration()
            {
                ArmEndpoint = "https://management.azure.com/",
                SubscriptionId = this.configuration["SubscriptionId"],
                MediaAccountName = this.configuration["MediaAccountName"],
                ResourceGroup = this.configuration["ResourceGroup"],
                AadTenantId = this.configuration["AadTenantId"],
                AadClientId = this.configuration["AadClientId"],
                AadSecret = this.configuration["AadSecret"]
            });

            return serviceProvider;
        }
    }
}
