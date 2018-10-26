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
            services.AddLogging();
            services.AddWorkflow();
            services.AddSingleton<IConfigurationService, DefaultConfigurationService>();
            //services.AddWorkflow(x => x.UseMongoDB(@"mongodb://localhost:27017", "workflow"));
            services.AddTransient<Tasks.IngestTask>();

            var serviceProvider = services.BuildServiceProvider();

            //config logging
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddDebug();

            serviceProvider.GetService<IConfigurationService>().Initialize(new MediaConfiguration()
            {
                ArmEndpoint = "https://management.azure.com/",
                SubscriptionId = this.configuration["SubscriptionId"],
                AccountName = this.configuration["AccountName"],
                ResourceGroup = this.configuration["ResourceGroup"],
                AadTenantId = this.configuration["AadTenantId"],
                AadClientId = this.configuration["AadClientId"],
                AadSecret = this.configuration["AadSecret"]
            });

            return serviceProvider;
        }
    }
}
