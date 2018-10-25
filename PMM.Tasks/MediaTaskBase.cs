namespace PMM.Tasks
{
    using System;

    using Microsoft.WindowsAzure.MediaServices.Client;

    using WorkflowCore.Primitives;

    using PMM.Core.Services.Contract;

    public abstract class MediaTaskBase : ContainerStepBody
    {
        protected CloudMediaContext context;

        public MediaTaskBase(IConfigurationService configurationService)
        {
            var tokenCredentials = new AzureAdTokenCredentials("%Your AAD Tenant Domain Here",
                new AzureAdClientSymmetricKey("%Client ID Here%", "%Client Secret Here%"),
                AzureEnvironments.AzureCloudEnvironment);
            var tokenProvider = new AzureAdTokenProvider(tokenCredentials);
            this.context = new CloudMediaContext(new Uri("%Your Rest API Endpoint Here%"), tokenProvider);
        }
    }
}
