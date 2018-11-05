namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Services.Contract;
    using ProcessMyMedia.Model;

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

        /// <summary>
        /// Gets or sets the analyzing parameters.
        /// </summary>
        /// <value>
        /// The analyzing parameters.
        /// </value>
        public AnalyzingParameters AnalyzingParameters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAnalyzerTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public MediaAnalyzerTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
        {
            this.AnalyzingParameters = new AnalyzingParameters();
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
            JobEntity job = context.PersistenceData as JobEntity;

            if (job == null)
            {
                //First call: stat analyse
                job = await this.mediaService.StartAnalyseAsync(this.AssetName, this.AnalyzingParameters);
            }

            this.Output.Job = job;

            if (!job.IsFinished)
            {
                return ExecutionResult.Sleep(TimeSpan.FromSeconds(60), job);
            }
            else if (job.Canceled)
            {
                throw new Exception("Analysing Job was canceled");
            }

            this.Output.Result = await this.mediaService.EndAnalyseAsync(job);

            return ExecutionResult.Next();
        }
    }
}
