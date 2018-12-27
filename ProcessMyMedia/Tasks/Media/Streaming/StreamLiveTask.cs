namespace ProcessMyMedia.Tasks
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Stream Live Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase{ProcessMyMedia.Model.StreamAssetTaskOutput}" />
    public class StreamLiveTask : MediaTaskBase<StreamLiveTaskOutput>
    {
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public StreamingOptions Options { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamLiveTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="delayService">The delay service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public StreamLiveTask(IMediaService mediaService, IDelayService delayService, ILoggerFactory loggerFactory) :
            base(mediaService,
                loggerFactory)
        {
            this.Options = new StreamingOptions();
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        protected override void ValidateInput()
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
