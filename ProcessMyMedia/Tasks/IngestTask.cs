namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Linq;
    using System.IO;
    using System.Threading.Tasks;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using Microsoft.Azure.Management.Media.Models;
    using Microsoft.Azure.Management.Media;
    using Microsoft.WindowsAzure.Storage.Blob;

    using ProcessMyMedia.Services.Contract;

    public class IngestTask : MediaTaskBase
    {
        public string AssetName { get; set; }

        public string AssetPath { get; set; }

        public IngestTask(IConfigurationService configurationService) : base(configurationService)
        {

        }

        public override ExecutionResult Run(IStepExecutionContext context)
        {
            //AssetCreationOptions assetCreationOptions = AssetCreationOptions.None;*
           
            //    this.client.Assets.CreateOrUpdateWithHttpMessagesAsync()

            //// Create a new asset and upload a local file using a single extension method.
            //IAsset asset = this.context.Assets.Create(this.AssetName, assetCreationOptions);

            //var accessPolicy = this.context.AccessPolicies.Create(this.AssetName, TimeSpan.FromDays(30),
            //    AccessPermissions.Write | AccessPermissions.List);

            //var locator = this.context.Locators.CreateLocator(LocatorType.Sas, asset, accessPolicy);

            //var blobTransferClient = new BlobTransferClient();
            //blobTransferClient.NumberOfConcurrentTransfers = 20;
            //blobTransferClient.ParallelTransferThreadCount = 20;

            //var uploadTasks = new List<Task>();

            //var assetFile = asset.AssetFiles.Create(Path.GetFileName(this.AssetPath));
            //uploadTasks.Add(assetFile.UploadAsync(this.AssetPath, blobTransferClient, locator, CancellationToken.None));
            //Task.WaitAll(uploadTasks.ToArray());

            //locator.Delete();
            //accessPolicy.Delete();

            return ExecutionResult.Next();
        }

        private static async Task<Asset> CreateInputAssetAsync(
        IAzureMediaServicesClient client,
        string resourceGroupName,
        string accountName,
        string assetName,
        string fileToUpload)
        {
            // In this example, we are assuming that the asset name is unique.
            //
            // If you already have an asset with the desired name, use the Assets.Get method
            // to get the existing asset. In Media Services v3, the Get method on entities returns null 
            // if the entity doesn't exist (a case-insensitive check on the name).

            // Call Media Services API to create an Asset.
            // This method creates a container in storage for the Asset.
            // The files (blobs) associated with the asset will be stored in this container.
            Asset asset = await client.Assets.CreateOrUpdateAsync(resourceGroupName, accountName, assetName, new Asset());

            // Use Media Services API to get back a response that contains
            // SAS URL for the Asset container into which to upload blobs.
            // That is where you would specify read-write permissions 
            // and the exparation time for the SAS URL.
            var response = await client.Assets.ListContainerSasAsync(
                resourceGroupName,
                accountName,
                assetName,
                permissions: AssetContainerPermission.ReadWrite,
                expiryTime: DateTime.UtcNow.AddHours(4).ToUniversalTime());

            var sasUri = new Uri(response.AssetContainerSasUrls.First());

            // Use Storage API to get a reference to the Asset container
            // that was created by calling Asset's CreateOrUpdate method.  
            CloudBlobContainer container = new CloudBlobContainer(sasUri);
            var blob = container.GetBlockBlobReference(Path.GetFileName(fileToUpload));

            // Use Strorage API to upload the file into the container in storage.
            await blob.UploadFromFileAsync(fileToUpload);

            return asset;
        }
    }
}
