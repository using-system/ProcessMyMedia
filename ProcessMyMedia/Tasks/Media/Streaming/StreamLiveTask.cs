namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Linq;
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
        /// Gets or sets the name of the live event.
        /// </summary>
        /// <value>
        /// The name of the live event.
        /// </value>
        public string LiveEventName { get; set; }

        /// <summary>
        /// Gets or sets the name of the asset.
        /// </summary>
        /// <value>
        /// The name of the asset.
        /// </value>
        public string AssetName { get; set; }

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

            string locatorName = Guid.NewGuid().ToString();

            await this.service.CreateStreamingLocatorAsync(locatorName, this.AssetName, this.Options);

            var urls = await this.service.GetStreamingUrlsAsync(locatorName);

            this.Output = new StreamLiveTaskOutput()
            {
                LocatorName = locatorName,
                LiveEventName = liveEvent.LiveEventName,
                LiveOutputName = liveEvent.LiveOutputName,
                StreamingUrls = urls.ToList(),
                IngestUrls = liveEvent.IngestUrls,
                PreviewUrls = liveEvent.PreviewUrls
            };

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
