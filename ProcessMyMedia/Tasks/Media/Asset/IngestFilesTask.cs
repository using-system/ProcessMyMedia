namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Ingest Files Task 
    /// </summary>
    public class IngestFilesTask : IngestTaskBase
    {
        /// <summary>
        /// Gets or sets the asset files path.
        /// </summary>
        /// <value>
        /// The asset files path.
        /// </value>
        public new List<string> AssetFiles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IngestFilesTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public IngestFilesTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
        {

        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        public override void ValidateInput()
        {
            base.ValidateInput();

            if (this.AssetFiles?.Count <= 0)
            {
                throw new ArgumentException($"{nameof(this.AssetFiles)} is required");
            }
        }

        /// <summary>
        /// Runs the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        public async override Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context)
        {
            base.AssetFiles = this.AssetFiles;

            return await base.RunMediaTaskAsync(context);
        }
    }
}
