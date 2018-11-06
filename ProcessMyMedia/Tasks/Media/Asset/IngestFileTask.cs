using System;

namespace ProcessMyMedia.Tasks
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

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
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public IngestFileTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
        {
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        public override void ValidateInput()
        {
            if (string.IsNullOrEmpty(this.AssetFilePath))
            {
                throw new ArgumentException($"{nameof(this.AssetFilePath)} is required");
            }
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
