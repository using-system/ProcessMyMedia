namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Services.Contract;
    using ProcessMyMedia.Model;

    /// <summary>
    /// Stream Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase" />
    public class StreamTask : MediaTaskBase<StreamTaskOutput>
    {
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
        /// Initializes a new instance of the <see cref="StreamTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="delayService">The delay service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public StreamTask(IMediaService mediaService, IDelayService delayService, ILoggerFactory loggerFactory) :
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
        protected async override Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context)
        {
            string locatorName = Guid.NewGuid().ToString();

            await this.service.CreateStreamingLocatorAsync(locatorName, this.AssetName, this.Options);

            var urls = await this.service.GetStreamingUrlsAsync(locatorName);

            this.Output = new StreamTaskOutput()
            {
                LocatorName = locatorName,
                StreamingUrls = urls.ToList()
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
