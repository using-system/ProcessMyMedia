namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;


    using Microsoft.Extensions.Logging;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;


    /// <summary>
    /// Ingest Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase" />
    public abstract class IngestFilesTaskBase : MediaTaskBase<IngestTaskOutput>
    {
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
        /// Gets or sets the asset path.
        /// </summary>
        /// <value>
        /// The asset path.
        /// </value>
        protected List<string> AssetFiles { get; set; }

        /// <summary>
        /// Gets or sets the name of the storage account.
        /// Optionnal. Default behavior : get the primary storage account associated to the media services account)
        /// </summary>
        /// <value>
        /// The name of the storage account.
        /// </value>
        public string StorageAccountName { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public Dictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IngestFilesTaskBase"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public IngestFilesTaskBase(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
        {
            this.AssetFiles = new List<string>();
            this.Metadata = new Dictionary<string, string>();
        }

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
        protected override async Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context)
        {
            AssetEntity asset = await this.service.CreateOrUpdateAssetAsync(this.AssetName,
                this.AssetDescription,
                this.StorageAccountName);

            await this.service.UploadFilesToAssetAsync(this.AssetName, this.AssetFiles, this.Metadata);

            this.Output = new IngestTaskOutput()
            {
                Asset =  asset
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
