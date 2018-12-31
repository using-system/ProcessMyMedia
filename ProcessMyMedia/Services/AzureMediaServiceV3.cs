namespace ProcessMyMedia.Services
{
    using System;
    using System.Text;
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
        private AmsConfiguration configuration;

        private AzureMediaServicesClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureMediaServiceV3"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AzureMediaServiceV3(AmsConfiguration configuration)
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
        /// Gets the asset.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="SecurityException">Not Authenticated</exception>
        public async Task<AssetEntity> GetAssetAsync(string name)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            try
            {

                var asset = await this.client.Assets.GetAsync(this.configuration.ResourceGroup,
                    this.configuration.MediaAccountName,
                    name);

                return asset.ToEntity();
            }
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
            }
        }

        /// <summary>
        /// Creates the or update asset.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <param name="assetDescription">The asset description.</param>
        /// <param name="storageAccountName">Name of the storage account.</param>
        /// <returns></returns>
        /// <exception cref="SecurityException">Not Authenticated</exception>
        public async Task<AssetEntity> CreateOrUpdateAssetAsync(string assetName, 
            string assetDescription = "",
            string storageAccountName = "")
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            try
            {
                Asset assetParameters = new Asset()
                {
                    Description = assetDescription,
                    StorageAccountName = storageAccountName
                };

                var asset = await client.Assets.CreateOrUpdateAsync(this.configuration.ResourceGroup, this.configuration.MediaAccountName, assetName, assetParameters);

                return asset.ToEntity();
            }
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
            }
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

            try
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

                if (metadata == null)
                {
                    return;
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
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
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
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            try
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
                            string path = Path.Combine(directoryToDownload, blob.Name);

                            downloadTasks.Add(blob.DownloadToFileAsync(path, FileMode.Create));
                        }
                    }

                    continuationToken = segment.ContinuationToken;
                }
                while (continuationToken != null);

                await Task.WhenAll(downloadTasks);
            }
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
            }

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

            try
            {
                var assetToDelete = await this.client.Assets.GetAsync(this.configuration.ResourceGroup, this.configuration.MediaAccountName, assetName);

                if (assetToDelete != null)
                {
                    await this.client.Assets.DeleteAsync(this.configuration.ResourceGroup, this.configuration.MediaAccountName, assetName);
                }
            }
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
            }

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
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            try
            {
                AssetEntity outputAsset = await this.CreateOrUpdateAssetAsync(Guid.NewGuid().ToString(),
                    assetDescription: $"Media Analysing for {assetName}");

                TransformOutput[] outputs = new TransformOutput[]
                {
                    new TransformOutput(parameters.ToAnalyzerPreset(), onError: OnErrorType.StopProcessingJob),
                };

                string transformName = $"MediaAnalysing-{Guid.NewGuid()}";
                Transform transform = await client.Transforms.CreateOrUpdateAsync
                    (this.configuration.ResourceGroup, this.configuration.MediaAccountName, transformName, outputs);

                Job job = await this.client.Jobs.CreateAsync(this.configuration.ResourceGroup,
                    this.configuration.MediaAccountName,
                    transformName,
                    $"job-{Guid.NewGuid()}",
                    new Job()
                    {
                        Input = new JobInputAsset(assetName),
                        Outputs = new List<JobOutput>
                        {
                            new JobOutputAsset(outputAsset.Name)
                        }
                    });

                return job.ToJobEntity(templateName: transformName);
            }
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
            }
        }

        /// <summary>
        /// Ends the analyse.
        /// </summary>
        /// <param name="job">The job associated to the analyse.</param>
        /// <returns></returns>
        public Task<AnalyzingResult> EndAnalyseAsync(JobEntity job)
        {
            var result = new AnalyzingResult();

            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            try
            {
                return Task.FromResult(new AnalyzingResult()
                {
                    OutputAssetName = job.Outputs.First().Name
                });
            }
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
            }
        }

        /// <summary>
        /// Starts the encode.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        /// <param name="encodingOutputs">The encoding outputs.</param>
        /// <param name="priority">The priority.</param>
        /// <returns></returns>
        /// <exception cref="SecurityException">Not Authenticated</exception>
        public async Task<JobEntity> StartEncodeAsync(IEnumerable<JobAssetEntity> inputs, 
            IEnumerable<EncodingOutputBase> encodingOutputs,
            JobPriority priority)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            try
            {
                TransformOutput[] transformOutputs = encodingOutputs.ToTransformOutputs(priority).ToArray();
                string transformName = $"Encoding-{Guid.NewGuid()}";
                Transform transform = await client.Transforms.CreateOrUpdateAsync
                    (this.configuration.ResourceGroup, this.configuration.MediaAccountName, transformName, transformOutputs);

                IList<JobOutput> jobOutputs = new List<JobOutput>();
                foreach (var jobOuput in encodingOutputs)
                {
                    string assetName = jobOuput.Name;
                    if (string.IsNullOrEmpty(assetName))
                    {
                        assetName = Guid.NewGuid().ToString();
                    }

                    await this.CreateOrUpdateAssetAsync(assetName);
                    jobOutputs.Add(new JobOutputAsset(assetName, label: jobOuput.Label));
                }

                Job job = await this.client.Jobs.CreateAsync(this.configuration.ResourceGroup,
                    this.configuration.MediaAccountName,
                    transformName,
                    $"job-{Guid.NewGuid()}",
                    new Job()
                    {
                        Input = inputs.ToJobInput(),
                        Outputs = jobOutputs,
                        Priority = priority.ToPrority()
                    });

                return job.ToJobEntity(templateName: transformName);
            }
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
            }
        }

        /// <summary>
        /// Gets the job asynchronous.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public async Task<JobEntity> GetJobAsync(string jobName, string templateName)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            try
            {
                var job = await this.client.Jobs.GetAsync(configuration.ResourceGroup,
                    this.configuration.MediaAccountName,
                    templateName, jobName);

                return job?.ToJobEntity(templateName: templateName);
            }
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
            }
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

            try
            {
                var jobToDelete = await this.GetJobAsync(jobName, templateName);

                if (jobToDelete != null)
                {
                    await client.Jobs.DeleteAsync(this.configuration.ResourceGroup,
                        this.configuration.MediaAccountName,
                        templateName,
                        jobName);
                }
            }
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
            }

        }

        /// <summary>
        /// Gets the template.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TemplateEntity> GetTemplateAsync(string name)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            try
            {
                var transform = await this.client.Transforms.GetAsync(this.configuration.ResourceGroup,
                    this.configuration.MediaAccountName,
                    name);

                return transform.ToTemplateEntity();
            }
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
            }


        }

        /// <summary>
        /// Deletes the template.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task DeleteTemplateAsync(string name)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            try
            {
                var templateToDelete = await this.GetTemplateAsync(name);

                if (templateToDelete != null)
                {
                    await client.Transforms.DeleteAsync(this.configuration.ResourceGroup,
                        this.configuration.MediaAccountName,
                        name);
                }
            }
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
            }
        }

        /// <summary>
        /// Creates the streaming locator.
        /// </summary>
        /// <param name="locatorName">Name of the locator.</param>
        /// <param name="assetName">Name of the asset.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        /// <exception cref="SecurityException">Not Authenticated</exception>
        public async Task CreateStreamingLocatorAsync(string locatorName, string assetName, Model.StreamingOptions options = null)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            try
            {
                var locator = await this.client.StreamingLocators.CreateAsync(
                    this.configuration.ResourceGroup,
                    this.configuration.MediaAccountName,
                    locatorName,
                    options.ToStreamingLocator(assetName));
            }
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
            }
        }

        /// <summary>
        /// Gets the streaming urls.
        /// </summary>
        /// <param name="locatorName">Name of the locator.</param>
        /// <param name="streamingEndpointName">Name of the streaming endpoint.</param>
        /// <returns></returns>
        /// <exception cref="SecurityException">Not Authenticated</exception>
        public async Task<IEnumerable<string>> GetStreamingUrlsAsync(string locatorName, string streamingEndpointName = "default")
        {
            IList<string> streamingUrls = new List<string>();

            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            try
            {
                var streamingEndpoint = await this.client.StreamingEndpoints.GetAsync(
                    this.configuration.ResourceGroup, 
                    this.configuration.MediaAccountName,
                    streamingEndpointName);

                if (streamingEndpoint != null)
                {
                    if (streamingEndpoint.ResourceState != StreamingEndpointResourceState.Running)
                    {
                        await this.client.StreamingEndpoints.StartAsync(
                            this.configuration.ResourceGroup, 
                            this.configuration.MediaAccountName, 
                            streamingEndpointName);
                    }
                }

                ListPathsResponse paths = await client.StreamingLocators.ListPathsAsync(
                    this.configuration.ResourceGroup, 
                    this.configuration.MediaAccountName, 
                    locatorName);

                foreach (StreamingPath path in paths.StreamingPaths)
                {
                    UriBuilder uriBuilder = new UriBuilder();
                    uriBuilder.Scheme = "https";
                    uriBuilder.Host = streamingEndpoint.HostName;

                    uriBuilder.Path = path.Paths[0];
                    streamingUrls.Add(uriBuilder.ToString());
                }

                return streamingUrls;

            }
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
            }
        }

        /// <summary>
        /// Creates the live event asynchronous.
        /// </summary>
        /// <param name="liveEventName">Name of the live event.</param>
        /// <param name="assetName">Name of the asset.</param>
        /// <returns></returns>
        /// <exception cref="SecurityException">Not Authenticated</exception>
        public async Task<Model.LiveEventEntity> CreateLiveEventAsync(string liveEventName, string assetName = null)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            try
            {
                if (string.IsNullOrEmpty(assetName))
                {
                    assetName = Guid.NewGuid().ToString();
                }

                LiveEventInputAccessControl liveEventInputAccess = new LiveEventInputAccessControl
                {
                    Ip = new IPAccessControl(
                            allow: new IPRange[]
                            {
                                new IPRange (
                                    name: "AllowAll",
                                    address: "0.0.0.0",
                                    subnetPrefixLength: 0
                                    )
                            })
                };

                LiveEventPreview liveEventPreview = new LiveEventPreview
                {
                    AccessControl = new LiveEventPreviewAccessControl(
                        ip: new IPAccessControl(
                            allow: new IPRange[]
                            {
                                new IPRange (
                                    name: "AllowAll",
                                    address: "0.0.0.0",
                                    subnetPrefixLength: 0
                                    )
                            }))
                };

                LiveEvent liveEvent = new LiveEvent(
                    //location: mediaService.Location,
                    description: "Sample LiveEvent for testing",
                    vanityUrl: false,
                    encoding: new LiveEventEncoding(
                        encodingType: LiveEventEncodingType.None,
                        presetName: null
                        ),
                    input: new LiveEventInput(LiveEventInputProtocol.RTMP, liveEventInputAccess),
                    preview: liveEventPreview,
                    streamOptions: new List<StreamOptionsFlag?>() { });

                liveEvent = await client.LiveEvents.CreateAsync(
                    this.configuration.ResourceGroup,
                    this.configuration.MediaAccountName,
                    liveEventName,
                    liveEvent,
                    autoStart: true);

                var asset = await client.Assets.CreateOrUpdateAsync(
                    this.configuration.ResourceGroup,
                    this.configuration.MediaAccountName,
                    assetName, new Asset());

                LiveOutput liveOutput = new LiveOutput(assetName: asset.Name, manifestName: "output", archiveWindowLength: TimeSpan.FromMinutes(10));
                liveOutput = await client.LiveOutputs.CreateAsync(
                    this.configuration.ResourceGroup,
                    this.configuration.MediaAccountName,
                    liveEventName,
                    Guid.NewGuid().ToString(),
                    liveOutput);

                return liveEvent.ToEntity();
            }
            catch (ApiErrorException exc)
            {
                throw GetApiException(exc);
            }
        }

        private static Exception GetApiException(ApiErrorException exc)
        {
            if (!string.IsNullOrEmpty(exc?.Body?.Error?.Message))
            {
                StringBuilder errorMessage = new StringBuilder();
                errorMessage.AppendLine($"{exc.Body.Error.Code} -{exc.Body.Error.Message}");
                if (exc.Body.Error.Details != null)
                {
                    foreach (var detail in exc.Body.Error.Details)
                    {
                        errorMessage.AppendLine($"{detail.Code} -{detail.Message}");
                    }
                }
                return new Exception(errorMessage.ToString());
            }

            return exc;
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
