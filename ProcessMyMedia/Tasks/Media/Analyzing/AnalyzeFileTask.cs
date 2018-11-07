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
    /// <seealso cref="ProcessMyMedia.Tasks.AnalyzeTaskBase" />
    public class AnalyzeFileTask : AnalyzeTaskBase
    {
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyzeFileTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public AnalyzeFileTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
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
        /// Runs the media analyse task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected async override Task RunMediaAnalyseTaskAsync(IStepExecutionContext context)
        {
            string assetName = $"input-{Guid.NewGuid()}";

            var asset = await mediaService.CreateOrUpdateAssetAsync(assetName);

            await mediaService.UploadFilesToAssetAsync(assetName, new[] { this.FilePath });

            this.AssetName = assetName;
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
                var assetToDelete = this.mediaService.GetAssetAsync(input);
                if (assetToDelete != null)
                {
                    await this.mediaService.DeleteAssetAsync(input);
                }
            }
        }
    }
}
