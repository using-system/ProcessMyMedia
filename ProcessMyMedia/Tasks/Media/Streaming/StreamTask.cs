namespace ProcessMyMedia.Tasks
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Services.Contract;


    /// <summary>
    /// Stream Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase" />
    public class StreamTask : MediaTaskBase
    {
        /// <summary>
        /// Gets or sets the name of the asset.
        /// </summary>
        /// <value>
        /// The name of the asset.
        /// </value>
        public string AssetName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="delayService">The delay service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public StreamTask(IMediaService mediaService, IDelayService delayService, ILoggerFactory loggerFactory) :
            base(mediaService,
                loggerFactory)
        {
        }

        /// <summary>
        /// Cleanups the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override Task Cleanup(IStepExecutionContext context)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        protected override void ValidateInput()
        {
            throw new System.NotImplementedException();
        }
    }
}
