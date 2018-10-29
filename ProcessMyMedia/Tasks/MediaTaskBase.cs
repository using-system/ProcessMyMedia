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
    /// <typeparam name="T">Output class</typeparam>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="WorkflowCore.Models.StepBodyAsync" />
    public abstract class MediaTaskBase<T> : MediaTaskBase
        where T : class   
    {
        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public T Output { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTaskBase" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public MediaTaskBase(MediaConfiguration configuration, ILoggerFactory loggerFactory)  :base(configuration, loggerFactory)
        {
            this.configuration = configuration;
            this.logger = loggerFactory.CreateLogger(this.GetType());
        }
    }


    /// <summary>
    /// Media Task Base
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase" />
    public abstract class MediaTaskBase : StepBodyAsync, IMediaTask
    {
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
            ExecutionResult result;
            ClientCredential clientCredential =
                new ClientCredential(this.configuration.AadClientId, this.configuration.AadSecret);

            var clientCredentials = await ApplicationTokenProvider.LoginSilentAsync(this.configuration.AadTenantId,
                clientCredential, ActiveDirectoryServiceSettings.Azure);

            using (var client = new AzureMediaServicesClient(new Uri(this.configuration.ArmEndpoint), clientCredentials)
            {
                SubscriptionId = this.configuration.SubscriptionId,
            })
            {
                result = await this.RunMediaTaskAsync(context, client);
            }

            return result;
        }

        /// <summary>
        /// Runs the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        public abstract Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context, AzureMediaServicesClient client);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}
