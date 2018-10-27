namespace ProcessMyMedia.Tasks
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Microsoft.Azure.Management.Media;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;

    /// <summary>
    /// Ingest Files Task 
    /// </summary>
    public class IngestFilesTask : IngestTaskBase
    {
        /// <summary>
        /// Gets or sets the asset files path.
        /// </summary>
        /// <value>
        /// The asset files path.
        /// </value>
        public new List<string> AssetFiles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IngestFilesTask" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory"></param>
        public IngestFilesTask(MediaConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {

        }

        /// <summary>
        /// Runs the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        public override Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context, AzureMediaServicesClient client)
        {
            return base.RunMediaTaskAsync(context, client);
        }
    }
}
