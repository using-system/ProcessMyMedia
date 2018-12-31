namespace ProcessMyMedia.Tasks
{
    using System;
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
    public class StreamLiveTask : StreamTaskBase<StreamLiveTaskOutput>
    {
        /// <summary>
        /// Gets or sets the name of the live event.
        /// </summary>
        /// <value>
        /// The name of the live event.
        /// </value>
        public string LiveEventName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamLiveTask" /> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public StreamLiveTask(IMediaService mediaService,ILoggerFactory loggerFactory) :
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
            if (string.IsNullOrEmpty(this.LiveEventName))
            {
                throw new ArgumentException($"{nameof(this.LiveEventName)} is required");
            }
        }

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected async override Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context)
        {
            var liveEvent = await this.service.CreateLiveEventAsync(this.LiveEventName, this.AssetName);

            await base.RunTaskAsync(context);

            this.Output.LiveEventName = liveEvent.LiveEventName;
            this.Output.LiveOutputName = liveEvent.LiveOutputName;
            this.Output.IngestUrls = liveEvent.IngestUrls;
            this.Output.PreviewUrls = liveEvent.PreviewUrls;

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
