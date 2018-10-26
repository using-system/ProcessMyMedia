

namespace ProcessMyMedia.Tasks
{
    using System;

    using WorkflowCore.Primitives;

    using ProcessMyMedia.Services.Contract;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Azure.Management.Media;
    using Microsoft.Rest.Azure.Authentication;

    using ProcessMyMedia.Model;

    public abstract class MediaTaskBase : ContainerStepBody
    {
        protected AzureMediaServicesClient client;

        protected MediaConfiguration configuration;

        public MediaTaskBase(IConfigurationService configurationService)
        {
            this.configuration = configurationService.Configuration;

            ClientCredential clientCredential = new ClientCredential(this.configuration.AadClientId, this.configuration.AadSecret);
            var clientCredentials = Microsoft.Rest.Azure.Authentication.ApplicationTokenProvider.LoginSilentAsync(this.configuration.AadTenantId, clientCredential, ActiveDirectoryServiceSettings.Azure).Result;
            this.client =  new AzureMediaServicesClient(new Uri(this.configuration.ArmEndpoint), clientCredentials)
            {
                SubscriptionId = this.configuration.SubscriptionId,
            };
        }
    }
}
