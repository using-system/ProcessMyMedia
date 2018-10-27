namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Extensions.Logging;
    using Microsoft.Azure.Management.Media;
    using Microsoft.Rest.Azure.Authentication;

    using ProcessMyMedia.Model;

    /// <summary>
    /// Media Task Base
    /// </summary>
    /// <seealso cref="WorkflowCore.Models.StepBodyAsync" />
    public abstract class MediaTaskBase : StepBodyAsync, IDisposable
    {
        protected AzureMediaServicesClient client;

        protected MediaConfiguration configuration;

        protected ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTaskBase" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public MediaTaskBase(MediaConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration;
            this.logger = loggerFactory.CreateLogger(this.GetType());
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            client?.Dispose();
            client = null;
        }
    }
}
