namespace ProcessMyMedia.Tasks
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Encode Media file base class
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.EncodeTaskBase" />
    public abstract class EncodeFileTaskBase : EncodeTaskBase
    {
        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodeFileTaskBase" /> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="delayService">The delay service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public EncodeFileTaskBase(IMediaService mediaService, IDelayService delayService, ILoggerFactory loggerFactory) 
            : base(mediaService, delayService, loggerFactory)
        {

        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <exception cref="ArgumentException">AssetName</exception>
        protected override void ValidateInput()
        {
            if (string.IsNullOrEmpty(this.FilePath))
            {
                throw new ArgumentException($"{nameof(this.FilePath)} is required");
            }

            if (!File.Exists(this.FilePath))
            {
                throw new ArgumentException($"File {this.FilePath} does not exsist");
            }
        }

        /// <summary>
        /// Runs the media encoding task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override async Task RunMediaEncodingTaskAsync(IStepExecutionContext context)
        {
            string assetName = $"input-{Guid.NewGuid()}";

            var asset = await mediaService.CreateOrUpdateAssetAsync(assetName);

            await mediaService.UploadFilesToAssetAsync(assetName, new[] { this.FilePath });

            this.Inputs.Add (new JobInputEntity(){ Name = assetName});
        }

        /// <summary>
        /// Cleanups the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected async override Task Cleanup(IStepExecutionContext context)
        {
            await base.Cleanup(context);

            JobEntity job = context.PersistenceData as JobEntity;

            if (job == null)
            {
                return;
            }

            foreach (var input in job.Inputs)
            {
                await this.mediaService.DeleteAssetAsync(input.Name);
            }
        }
    }
}
