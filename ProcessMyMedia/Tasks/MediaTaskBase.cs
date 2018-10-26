

namespace ProcessMyMedia.Tasks
{
    using System;

    using WorkflowCore.Models;

    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Azure.Management.Media;
    using Microsoft.Rest.Azure.Authentication;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;


    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="WorkflowCore.Models.StepBodyAsync" />
    public abstract class MediaTaskBase : StepBodyAsync
    {
        protected AzureMediaServicesClient client;

        protected MediaConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTaskBase"/> class.
        /// </summary>
        /// <param name="configurationService">The configuration service.</param>
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
