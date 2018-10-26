﻿namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using WorkflowCore.Interface;
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
        }

        /// <summary>
        /// Runs the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            ClientCredential clientCredential =
                new ClientCredential(this.configuration.AadClientId, this.configuration.AadSecret);
            var clientCredentials = await ApplicationTokenProvider.LoginSilentAsync(this.configuration.AadTenantId,
                clientCredential, ActiveDirectoryServiceSettings.Azure);
            this.client = new AzureMediaServicesClient(new Uri(this.configuration.ArmEndpoint), clientCredentials)
            {
                SubscriptionId = this.configuration.SubscriptionId,
            };

            return await this.RunMediaTaskAsync(context);
        }

        /// <summary>
        /// Runs the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public abstract Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context);
    }
}
