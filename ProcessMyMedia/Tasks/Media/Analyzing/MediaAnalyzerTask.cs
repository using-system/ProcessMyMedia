namespace ProcessMyMedia.Tasks.Media.Analyzing
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model.Tasks;
    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Media Analyzer Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase{ProcessMyMedia.Model.Tasks.MediaAnalyzerTaskOutput}" />
    public class MediaAnalyzerTask : MediaTaskBase<MediaAnalyzerTaskOutput>
    {
        /***
         * https://docs.microsoft.com/en-us/azure/media-services/latest/analyze-videos-tutorial-with-api
         * https://github.com/Azure-Samples/media-services-v3-dotnet-tutorials/tree/master/AMSV3Tutorials/AnalyzeVideos
         */
        /// <summary>
        /// Gets or sets the name of the asset.
        /// </summary>
        /// <value>
        /// The name of the asset.
        /// </value>
        public string AssetName { get; set; }

        public MediaAnalyzerTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
        {

        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <exception cref="ArgumentException">AssetName</exception>
        public override void ValidateInput()
        {
            if (string.IsNullOrEmpty(this.AssetName))
            {
                throw new ArgumentException($"{nameof(this.AssetName)} is required");
            }
        }

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override async Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context)
        {
            return ExecutionResult.Next();
        }
    }
}
