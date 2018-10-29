using System;

namespace ProcessMyMedia.Tasks
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Microsoft.Azure.Management.Media;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;

    /// <summary>
    /// Ingest File Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.IngestTaskBase" />
    public class IngestFileTask : IngestTaskBase
    {
        /// <summary>
        /// Gets or sets the asset file path.
        /// </summary>
        /// <value>
        /// The asset file path.
        /// </value>
        public string  AssetFilePath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IngestFileTask" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory"></param>
        public IngestFileTask(MediaConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        public override void ValidateInput()
        {
            base.ValidateInput();

            if (string.IsNullOrEmpty(this.AssetFilePath))
            {
                throw new ArgumentException($"{nameof(this.AssetFilePath)} is required");
            }
        }

        /// <summary>
        /// Runs the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        public override async Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context, AzureMediaServicesClient client)
        {
            this.AssetFiles.Add(this.AssetFilePath);

            return await base.RunMediaTaskAsync(context, client);
        }
    }
}
