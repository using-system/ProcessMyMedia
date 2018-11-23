namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;


    using Microsoft.Extensions.Logging;

    using ProcessMyMedia.Services.Contract;


    /// <summary>
    /// Ingest Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase" />
    public abstract class IngestFilesTaskBase : IngestTask
    {
        /// <summary>
        /// Gets or sets the asset path.
        /// </summary>
        /// <value>
        /// The asset path.
        /// </value>
        protected List<string> AssetFiles { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public Dictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IngestFilesTaskBase"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public IngestFilesTaskBase(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
        {
            this.AssetFiles = new List<string>();
            this.Metadata = new Dictionary<string, string>();
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        protected override void ValidateInput()
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
        protected override async Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context)
        {
            await base.RunTaskAsync(context);

            await this.service.UploadFilesToAssetAsync(this.AssetName, this.AssetFiles, this.Metadata);

            return ExecutionResult.Next();
        }

        /// <summary>
        /// Cleanups the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected async override Task Cleanup(IStepExecutionContext context)
        {
            if (this.onError && !String.IsNullOrEmpty(this.AssetName))
            {
                await this.service.DeleteAssetAsync(this.AssetName);
            }
        }

    }
}
