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

            services.AddMediaServices(configuration: new AmsConfiguration()
            {
                ArmEndpoint = "https://management.azure.com/",
                SubscriptionId = this.configuration["MediaServices:SubscriptionId"],
                MediaAccountName = this.configuration["MediaServices:MediaAccountName"],
                ResourceGroup = this.configuration["MediaServices:ResourceGroup"],
                AadTenantId = this.configuration["MediaServices:AadTenantId"],
                AadClientId = this.configuration["MediaServices:AadClientId"],
                AadSecret = this.configuration["MediaServices:AadSecret"]
            });

            services.AddDataFactoryServices(configuration: new AdfConfiguration()
            {
                ArmEndpoint = "https://management.azure.com/",
                SubscriptionId = this.configuration["DataFactory:SubscriptionId"],
                FactoryName = this.configuration["DataFactory:FactoryName"],
                ResourceGroup = this.configuration["DataFactory:ResourceGroup"],
                AadTenantId = this.configuration["DataFactory:AadTenantId"],
                AadClientId = this.configuration["DataFactory:AadClientId"],
                AadSecret = this.configuration["DataFactory:AadSecret"]
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
