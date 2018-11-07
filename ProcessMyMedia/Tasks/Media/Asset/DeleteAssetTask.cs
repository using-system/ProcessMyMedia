namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Delete Asset Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase" />
    public class DeleteAssetTask : MediaTaskBase
    {
        /// <summary>
        /// Gets or sets the name of the asset.
        /// </summary>
        /// <value>
        /// The name of the asset.
        /// </value>
        public string AssetName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteAssetTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public DeleteAssetTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
        {

        }

        /// <summary>
        /// Validates the input.
        /// </summary>
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
        protected override async Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context)
        {
            await this.mediaService.DeleteAssetAsync(this.AssetName);

            return ExecutionResult.Next();
        }

        /// <summary>
        /// Cleanups the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected async override Task Cleanup(IStepExecutionContext context)
        {

        }
    }
}
