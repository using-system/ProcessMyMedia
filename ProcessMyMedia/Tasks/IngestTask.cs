namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.WindowsAzure.MediaServices.Client;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

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
            AssetCreationOptions assetCreationOptions = AssetCreationOptions.None;

            // Create a new asset and upload a local file using a single extension method.
            IAsset asset = this.context.Assets.Create(this.AssetName, assetCreationOptions);

            var accessPolicy = this.context.AccessPolicies.Create(this.AssetName, TimeSpan.FromDays(30),
                AccessPermissions.Write | AccessPermissions.List);

            var locator = this.context.Locators.CreateLocator(LocatorType.Sas, asset, accessPolicy);

            var blobTransferClient = new BlobTransferClient();
            blobTransferClient.NumberOfConcurrentTransfers = 20;
            blobTransferClient.ParallelTransferThreadCount = 20;

            var uploadTasks = new List<Task>();

            var assetFile = asset.AssetFiles.Create(Path.GetFileName(this.AssetPath));
            uploadTasks.Add(assetFile.UploadAsync(this.AssetPath, blobTransferClient, locator, CancellationToken.None));
            Task.WaitAll(uploadTasks.ToArray());

            locator.Delete();
            accessPolicy.Delete();

            return ExecutionResult.Next();
        }
    }
}
