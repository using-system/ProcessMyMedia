namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Services.Contract;

    public class DownloadAssetTask : MediaTaskBase
    {
        /// <summary>
        /// Gets or sets the name of the asset.
        /// </summary>
        /// <value>
        /// The name of the asset.
        /// </value>
        public string AssetName { get; set; }

        /// <summary>
        /// Gets or sets the directory to download.
        /// </summary>
        /// <value>
        /// The directory to download.
        /// </value>
        public string DirectoryToDownload { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadAssetTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public DownloadAssetTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
        {

        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// AssetName
        /// or
        /// DirectoryToDownload
        /// </exception>
        protected override void ValidateInput()
        {
            if (string.IsNullOrEmpty(this.AssetName))
            {
                throw new ArgumentException($"{nameof(this.AssetName)} is required");
            }

            if (string.IsNullOrEmpty(this.DirectoryToDownload))
            {
                throw new ArgumentException($"{nameof(this.DirectoryToDownload)} is required");
            }
        }

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected async override Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context)
        {
            this.logger.LogInformation($"Upload files from the asset {this.AssetName} to {this.DirectoryToDownload}");

            await this.service.DownloadFilesAsync(this.AssetName, this.DirectoryToDownload);

            return ExecutionResult.Next();
        }

        /// <summary>
        /// Cleanups the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override Task Cleanup(IStepExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}
