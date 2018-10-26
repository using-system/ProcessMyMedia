namespace ProcessMyMedia.Tasks
{
    using System.Threading.Tasks;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Services.Contract;

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
        /// Initializes a new instance of the <see cref="IngestFileTask"/> class.
        /// </summary>
        /// <param name="configurationService">The configuration service.</param>
        public IngestFileTask(IConfigurationService configurationService) : base(configurationService)
        {
        }

        /// <summary>
        /// Runs the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override async Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context)
        {
            this.AssetFiles.Add(this.AssetFilePath);

            return await base.RunMediaTaskAsync(context);
        }
    }
}
