namespace ProcessMyMedia.Services
{
    using System;
    using System.Security;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.Azure.Management.DataFactory;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Rest.Azure.Authentication;
    using Microsoft.Azure.Management.DataFactory.Models;

    using Newtonsoft.Json.Linq;

    using ProcessMyMedia.Extensions;

    /// <summary>
    /// Azure Data Factory client for V2 API
    /// https://docs.microsoft.com/en-us/azure/data-factory/connector-ftp
    /// https://docs.microsoft.com/en-us/azure/data-factory/connector-file-system
    /// https://docs.microsoft.com/en-us/azure/data-factory/connector-azure-blob-storage
    /// https://docs.microsoft.com/en-us/azure/data-factory/concepts-pipelines-activities
    /// https://docs.microsoft.com/en-us/azure/data-factory/concepts-pipeline-execution-triggers
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Services.Contract.IDataFactoryService" />
    public class AzureDataFactoryServiceV2 : Contract.IDataFactoryService
    {
        private Model.AdfConfiguration configuration;

        private DataFactoryManagementClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureDataFactoryServiceV2" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public AzureDataFactoryServiceV2(Model.AdfConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Authentications the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task AuthAsync()
        {
            ClientCredential clientCredential =
                new ClientCredential(this.configuration.AadClientId, this.configuration.AadSecret);

            var clientCredentials = await ApplicationTokenProvider.LoginSilentAsync(this.configuration.AadTenantId,
                clientCredential, ActiveDirectoryServiceSettings.Azure);

            this.client = new DataFactoryManagementClient(new Uri(this.configuration.ArmEndpoint), clientCredentials)
            {
                SubscriptionId = this.configuration.SubscriptionId,
            };
        }

        /// <summary>
        /// Gets the linked service.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task<Model.LinkedServiceEntity> GetLinkedServiceAsync(string name)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            var linkedService = await this.client.LinkedServices.GetAsync(
                this.configuration.ResourceGroup,
                this.configuration.FactoryName,
                name);

            return linkedService.ToEntity();
        }

        /// <summary>
        /// Adds the linked service.
        /// https://docs.microsoft.com/en-us/rest/api/datafactory/linkedservices/createorupdate
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="typeProperties">The properties.</param>
        /// <returns></returns>
        public async Task<Model.LinkedServiceEntity> CreateOrUpdateLinkedServiceAsync(string name, string type, object typeProperties)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            var linkedService = await this.client.LinkedServices.CreateOrUpdateAsync(
                this.configuration.ResourceGroup,
                this.configuration.FactoryName,
                name,
                new LinkedServiceResource(new LinkedService(additionalProperties:new Dictionary<string, object>()
                    {
                        {"type", type},
                        {"typeProperties", JObject.FromObject(typeProperties)}
                    }),
                    type:type));

            return linkedService.ToEntity();
        }

        /// <summary>
        /// Gets the dataset.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task<Model.DatasetEntity> GetDatasetAsync(string name)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            var dataset = await this.client.Datasets.GetAsync(
                this.configuration.ResourceGroup,
                this.configuration.FactoryName,
                name);

            return dataset.ToDatasetEntity();
        }

        /// <summary>
        /// Creates the or update dataset.
        /// https://docs.microsoft.com/en-us/rest/api/datafactory/datasets/createorupdate
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns></returns>
        public async Task<Model.DatasetEntity> CreateOrUpdateDatasetAsync(Model.DatasetEntity dataset)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            var response = await this.client.Datasets.CreateOrUpdateAsync(
                this.configuration.ResourceGroup,
                this.configuration.FactoryName,
                dataset.Name,
                new DatasetResource(
                    new Dataset(new LinkedServiceReference(dataset.LinkedServiceName), additionalProperties: new Dictionary<string, object>()
                    {
                        {"type", dataset.Type},
                        {"typeProperties", JObject.FromObject(dataset.TypeProperties)}
                    }, description: dataset.Description)));

            return response.ToDatasetEntity();
        }

        /// <summary>
        /// Deletes the dataset.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task DeleteDatasetAsync(string name)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            var datasetToDelete = await this.GetDatasetAsync(name);
            if (datasetToDelete != null)
            {
                await this.client.Datasets.DeleteAsync(
                    this.configuration.ResourceGroup,
                    this.configuration.FactoryName,
                    name);
            }
        }

        /// <summary>
        /// Gets the pipeline.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task<Model.DataPipelineEntity> GetPipelineAsync(string name)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            var pipeline = await this.client.Pipelines.GetAsync(
                this.configuration.ResourceGroup,
                this.configuration.FactoryName,
                name);

            return pipeline.ToPipelineEntity();
        }

        /// <summary>
        /// Creates the or update pipeliney.
        /// </summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public async Task CreateOrUpdatePipelineyAsync(Model.DataPipelineEntity pipeline)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            await this.client.Pipelines.CreateOrUpdateAsync(
                this.configuration.ResourceGroup,
                this.configuration.FactoryName,
                pipeline.Name,
                new PipelineResource(activities: pipeline.ToActivities().ToList(), 
                    description: pipeline.Description));
        }

        /// <summary>
        /// Deletes the pipeline.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task DeletePipelineAsync(string name)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            var pipelineToDelete = await this.GetPipelineAsync(name);
            if (pipelineToDelete != null)
            {
                await this.client.Pipelines.DeleteAsync(
                    this.configuration.ResourceGroup,
                    this.configuration.FactoryName,
                    name);
            }
        }

        /// <summary>
        /// Runs the pipeline.
        /// </summary>
        /// <param name="pipelineName">Name of the pipeline.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public async Task<string> RunPipelineAsync(string pipelineName, Dictionary<string, object> properties = null)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            var response = await this.client.Pipelines.CreateRunAsync(
                this.configuration.ResourceGroup,
                this.configuration.FactoryName,
                pipelineName,
                parameters: properties);

            return response.RunId;
        }

        /// <summary>
        /// Getpipelines the run.
        /// </summary>
        /// <param name="runID">The run identifier.</param>
        /// <returns></returns>
        public async Task<Model.DataPipelineRunEntity> GetPipelineRunAsync(string runID)
        {
            if (this.client == null)
            {
                throw new SecurityException("Not Authenticated");
            }

            var run = await this.client.PipelineRuns.GetAsync(
                this.configuration.ResourceGroup,
                this.configuration.FactoryName,
                runID);

            var activities = await this.client.ActivityRuns.QueryByPipelineRunAsync(
                configuration.ResourceGroup, 
                configuration.FactoryName, 
                runID, 
                new RunFilterParameters(DateTime.UtcNow.AddMinutes(-10), DateTime.UtcNow.AddMinutes(10)));

            return run.ToPipelineRunEntity(activities.Value);
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
