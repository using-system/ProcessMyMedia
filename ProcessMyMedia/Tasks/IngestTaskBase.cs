namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.IO;
    using System.Threading.Tasks;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using Microsoft.Azure.Management.Media.Models;
    using Microsoft.Azure.Management.Media;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.Extensions.Logging;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Extensions;

    /// <summary>
    /// Ingest Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase" />
    public abstract class IngestTaskBase : MediaTaskBase<IngestTaskOutput>
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
        /// Initializes a new instance of the <see cref="IngestTaskBase" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory"></param>
        public IngestTaskBase(MediaConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
            this.AssetFiles = new List<string>();
            this.Metadata = new Dictionary<string, string>();
        }

        /// <summary>
        /// Runs the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        public override async Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context, AzureMediaServicesClient client)
        {
            Asset assetParameters = new Asset()
            {
                StorageAccountName = this.StorageAccountName,
                Description = this.AssetDescription                
            };

            Asset asset = await client.Assets.CreateOrUpdateAsync(this.configuration.ResourceGroup, this.configuration.MediaAccountName, this.AssetName, assetParameters);

            var response = await client.Assets.ListContainerSasAsync(
                this.configuration.ResourceGroup,
                this.configuration.MediaAccountName,
                this.AssetName,
                permissions: AssetContainerPermission.ReadWrite,
                expiryTime: DateTime.UtcNow.AddHours(4).ToUniversalTime());
            var sasUri = new Uri(response.AssetContainerSasUrls.First());

            CloudBlobContainer container = new CloudBlobContainer(sasUri);
            foreach (string assetPath in this.AssetFiles)
            {
                var blob = container.GetBlockBlobReference(Path.GetFileName(assetPath));
                await blob.UploadFromFileAsync(assetPath);              
            }

            foreach (var metadata in this.Metadata)
            {
                if (container.Metadata.ContainsKey(metadata.Key))
                {
                    container.Metadata[metadata.Key] = metadata.Value;
                }
                else
                {
                    container.Metadata.Add(metadata.Key, metadata.Value);
                }
            }

            if (container.Metadata.Count > 0)
            {
                await container.SetMetadataAsync();
            }

            this.Output = new IngestTaskOutput()
            {
                Asset =  asset.ToEntity()
            };

            return ExecutionResult.Next();
        }

    }
}
