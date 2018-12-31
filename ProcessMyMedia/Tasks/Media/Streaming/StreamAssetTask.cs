namespace ProcessMyMedia.Tasks
{
    using System;

    using Microsoft.Extensions.Logging;

    using ProcessMyMedia.Services.Contract;
    using ProcessMyMedia.Model;

    /// <summary>
    /// Stream Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase" />
    public class StreamAssetTask : StreamTaskBase<StreamAssetTaskOutput>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StreamAssetTask" /> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        public StreamAssetTask(IMediaService mediaService, ILoggerFactory loggerFactory) :
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
    }
}
