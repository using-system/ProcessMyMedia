namespace ProcessMyMedia.Tasks
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Media Analyzer Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase{ProcessMyMedia.Model.Tasks.MediaAnalyzerTaskOutput}" />
    public class AnalyzeMediaAssetTask : AnalyzeMediaTaskBase
    {
        /// <summary>
        /// Gets or sets the name of the asset.
        /// </summary>
        /// <value>
        /// The name of the asset.
        /// </value>
        public new string AssetName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyzeMediaAssetTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public AnalyzeMediaAssetTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
        {

        }

        public override async Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context)
        {
            base.AssetName = this.AssetName;

            return await base.RunMediaTaskAsync(context);
        }

    }
}
