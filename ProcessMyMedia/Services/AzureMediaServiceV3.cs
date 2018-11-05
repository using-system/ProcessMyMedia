namespace ProcessMyMedia.Services
{
    using System;
    using System.Security;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Azure.Management.Media;
    using Microsoft.Azure.Management.Media.Models;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Rest.Azure.Authentication;
    using Microsoft.WindowsAzure.Storage.Blob;

    using ProcessMyMedia.Model;

    using ProcessMyMedia.Extensions;

    /// <summary>
    /// Azure Media Service client with V3 API
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Services.Contract.IMediaService" />
    public class AzureMediaServiceV3 : Contract.IMediaService
    {
        private WamsConfiguration configuration;

        private AzureMediaServicesClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureMediaServiceV3"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AzureMediaServiceV3(WamsConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Authenticate.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task AuthAsync()
        {
            ClientCredential clientCredential =
                new ClientCredential(this.configuration.AadClientId, this.configuration.AadSecret);

            var clientCredentials = await ApplicationTokenProvider.LoginSilentAsync(this.configuration.AadTenantId,
                clientCredential, ActiveDirectoryServiceSettings.Azure);

            this.client = new AzureMediaServicesClient(new Uri(this.configuration.ArmEndpoint), clientCredentials)
            {
                SubscriptionId = this.configuration.SubscriptionId,
            };
        }

        /// <summary>
        /// Creates the or update asset.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <param name="assetDescription">The asset description.</param>
        /// <param name="storageAccountName">Name of the storage account.</param>
        /// <returns></returns>
        public async Task<AssetEntity> CreateOrUpdateAssetAsync(string assetName, 
            string assetDescription = "",
            string storageAccountName = "")
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            Asset assetParameters = new Asset()
            {
                StorageAccountName = storageAccountName,
                Description = assetDescription
            };

            var asset = await client.Assets.CreateOrUpdateAsync(this.configuration.ResourceGroup, this.configuration.MediaAccountName, assetName, assetParameters);

            return asset.ToEntity();
        }

        /// <summary>
        /// Uploads the files to asset.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <param name="files">The files.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns></returns>
        /// <exception cref="SecurityException">Not Authenticated</exception>
        public async Task UploadFilesToAssetAsync(string assetName,
            IEnumerable<string> files,
            IDictionary<string, string> metadata = null)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            var response = await this.client.Assets.ListContainerSasAsync(
                this.configuration.ResourceGroup,
                this.configuration.MediaAccountName,
                assetName,
                permissions: AssetContainerPermission.ReadWrite,
                expiryTime: DateTime.UtcNow.AddHours(4).ToUniversalTime());
            var sasUri = new Uri(response.AssetContainerSasUrls.First());

            CloudBlobContainer container = new CloudBlobContainer(sasUri);
            foreach (string assetPath in files)
            {
                var blob = container.GetBlockBlobReference(Path.GetFileName(assetPath));
                await blob.UploadFromFileAsync(assetPath);
            }

            foreach (var entry in metadata)
            {
                if (container.Metadata.ContainsKey(entry.Key))
                {
                    container.Metadata[entry.Key] = entry.Value;
                }
                else
                {
                    container.Metadata.Add(entry.Key, entry.Value);
                }
            }
        }

        /// <summary>
        /// Downloads the files.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <param name="directoryToDownload">The directory to download.</param>
        /// <returns></returns>
        public async Task DownloadFilesAsync(string assetName, string directoryToDownload)
        {
            if (!Directory.Exists(directoryToDownload))
            {
                Directory.CreateDirectory(directoryToDownload);
            }

            AssetContainerSas assetContainerSas = await client.Assets.ListContainerSasAsync(
                this.configuration.ResourceGroup,
                this.configuration.MediaAccountName,
                assetName,
                permissions: AssetContainerPermission.Read,
                expiryTime: DateTime.UtcNow.AddHours(1).ToUniversalTime());

            Uri containerSasUrl = new Uri(assetContainerSas.AssetContainerSasUrls.FirstOrDefault());
            CloudBlobContainer container = new CloudBlobContainer(containerSasUrl);

            string directory = Path.Combine(directoryToDownload, assetName);
            Directory.CreateDirectory(directory);

            BlobContinuationToken continuationToken = null;
            IList<Task> downloadTasks = new List<Task>();

            do
            {
                BlobResultSegment segment = await container.ListBlobsSegmentedAsync(null, true, BlobListingDetails.None, null, continuationToken, null, null);

                foreach (IListBlobItem blobItem in segment.Results)
                {
                    CloudBlockBlob blob = blobItem as CloudBlockBlob;
                    if (blob != null)
                    {
                        string path = Path.Combine(directory, blob.Name);

                        downloadTasks.Add(blob.DownloadToFileAsync(path, FileMode.Create));
                    }
                }

                continuationToken = segment.ContinuationToken;
            }
            while (continuationToken != null);

            await Task.WhenAll(downloadTasks);
        }

        /// <summary>
        /// Deletes the asset.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <returns></returns>
        public async Task DeleteAssetAsync(string assetName)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            await this.client.Assets.DeleteAsync(this.configuration.ResourceGroup, this.configuration.MediaAccountName, assetName);
        }

        /// <summary>
        /// Annalyses the specified asset name.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<JobEntity> StartAnalyseAsync(string assetName, AnalyzingParameters parameters)
        {
            AssetEntity outputAsset = await this.CreateOrUpdateAssetAsync($"{assetName}{Guid.NewGuid()}", 
                assetDescription: $"Media Analysing for {assetName}");

            string transformName = $"MediaAnalysing-{Guid.NewGuid()}";
            TransformOutput[] outputs = new TransformOutput[]
            {
                new TransformOutput(parameters.ToAnalyzerPreset()),
            };
            Transform transform = await client.Transforms.CreateOrUpdateAsync
                (this.configuration.ResourceGroup, this.configuration.MediaAccountName, transformName, outputs);

            Job job = await this.client.Jobs.CreateAsync(this.configuration.ResourceGroup,
                this.configuration.MediaAccountName,
                transformName,
                $"job-{Guid.NewGuid()}",
                new Job()
                {
                    Input = new JobInputAsset(assetName),
                    Outputs =
                    {
                        new JobOutputAsset(outputAsset.Name)
                    }
                });

            return job.ToJobEntity(templateName:transformName);
        }

        /// <summary>
        /// Gets the job asynchronous.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public async Task<JobEntity> GetJobAsync(string jobName, string templateName)
        {
            var job = await this.client.Jobs.GetAsync(this.configuration.ResourceGroup,
                this.configuration.MediaAccountName,
                jobName,
                templateName);

            return job.ToJobEntity(templateName:templateName);
        }

        /// <summary>
        /// Deletes the job.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        /// <exception cref="SecurityException">Not Authenticated</exception>
        public async Task DeleteJobAsync(string jobName, string templateName)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            await client.Jobs.DeleteAsync(this.configuration.ResourceGroup,
                this.configuration.MediaAccountName,
                templateName,
                jobName);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            this.client?.Dispose();
        }

    }
}
