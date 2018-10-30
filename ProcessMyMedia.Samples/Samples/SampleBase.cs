namespace ProcessMyMedia.Samples
{
    using System;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using ProcessMyMedia.Model;

    public abstract class SampleBase
    {
        internal const string WORKFLOW_NAME = "SampleWorklow";

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

            services.AddMediaServices(configuration: new MediaConfiguration()
            {
                ArmEndpoint = "https://management.azure.com/",
                SubscriptionId = this.configuration["SubscriptionId"],
                MediaAccountName = this.configuration["MediaAccountName"],
                ResourceGroup = this.configuration["ResourceGroup"],
                AadTenantId = this.configuration["AadTenantId"],
                AadClientId = this.configuration["AadClientId"],
                AadSecret = this.configuration["AadSecret"]
            });

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddConsole();
                builder.AddDebug();
            });

            return services.BuildServiceProvider();
        }
    }
}
