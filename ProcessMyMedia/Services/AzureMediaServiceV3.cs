﻿using System.Security;

namespace ProcessMyMedia.Services
{
    using System;
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

        public async Task UploadFilesToAssetAsync(string assetName,
            IEnumerable<string> files,
            IDictionary<string, string> metadata = null)
        {
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
        /// Deletes the asset.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <returns></returns>
        public async Task DeleteAssetAsync(string assetName)
        {
            await this.client.Assets.DeleteAsync(this.configuration.ResourceGroup, this.configuration.MediaAccountName, assetName);
        }

        /// <summary>
        /// Deletes the job.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="transformationName">Name of the transformation.</param>
        /// <returns></returns>
        public async Task DeleteJobAsync(string jobName, string transformationName)
        {
            await client.Jobs.DeleteAsync(this.configuration.ResourceGroup,
                this.configuration.MediaAccountName,
                transformationName,
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
