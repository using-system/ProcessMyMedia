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
    /// Analyze Media File Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.AnalyzeMediaTaskBase" />
    public class AnalyzeMediaFileTask : AnalyzeMediaTaskBase
    {
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyzeMediaFileTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public AnalyzeMediaFileTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
        {

        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <exception cref="ArgumentException">AssetName</exception>
        public override void ValidateInput()
        {
            if (string.IsNullOrEmpty(this.FilePath))
            {
                throw new ArgumentException($"{nameof(this.FilePath)} is required");
            }
        }

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override async Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context)
        {
            string assetName = $"input-{Guid.NewGuid()}";

            var asset = await mediaService.CreateOrUpdateAssetAsync(assetName);

            await mediaService.UploadFilesToAssetAsync(assetName, new[] { this.FilePath });

            this.AssetName = assetName;

            return await base.RunMediaTaskAsync(context);
        }

        /// <summary>
        /// Cleanups the specified job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        protected async override Task Cleanup(JobEntity job)
        {
            await base.Cleanup(job);

            foreach (var input in job.InputAssetNames)
            {
                await this.mediaService.DeleteAssetAsync(input);
            }
        }
    }
}
