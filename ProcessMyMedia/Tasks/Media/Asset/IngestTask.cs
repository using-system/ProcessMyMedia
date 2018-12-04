namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;

    public class IngestTask : MediaTaskBase<IngestTaskOutput>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IngestTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public IngestTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
        {

        }

        /// <summary>
        /// Gets or sets the name of the asset.
        /// </summary>
        /// <value>
        /// The name of the asset.
        /// </value>
        public string AssetName { get; set; }

        /// <summary>
        /// Gets or sets the asset description. (Optionnal)
        /// </summary>
        /// <value>
        /// The asset description.
        /// </value>
        public string AssetDescription { get; set; }

        /// <summary>
        /// Gets or sets the name of the storage account.
        /// Optionnal. Default behavior : get the primary storage account associated to the media services account)
        /// </summary>
        /// <value>
        /// The name of the storage account.
        /// </value>
        public string StorageAccountName { get; set; }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
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
            this.logger.LogInformation($"Create the asset {this.AssetName}");

            AssetEntity asset = await this.service.CreateOrUpdateAssetAsync(this.AssetName,
                assetDescription: this.AssetDescription,
                storageAccountName: this.StorageAccountName);

            this.Output = new IngestTaskOutput()
            {
                Asset = asset
            };

            return ExecutionResult.Next();
        }

        /// <summary>
        /// Cleanups the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected async override Task Cleanup(IStepExecutionContext context)
        {
            if (this.onError && !String.IsNullOrEmpty(this.AssetName))
            {
                await this.service.DeleteAssetAsync(this.AssetName);
            }
        }

    }
}
