namespace ProcessMyMedia.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.Azure.Management.DataFactory;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Rest.Azure.Authentication;
    using Microsoft.Azure.Management.DataFactory.Models;

    using ProcessMyMedia.Extensions;

    /// <summary>
    /// Azure Data Factory client for V2 API
    /// https://docs.microsoft.com/en-us/azure/data-factory/connector-ftp
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
        /// Adds the linked service.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public async Task CreateOrUpdateLinkedServiceAsync(string name, string type, Dictionary<string, object> properties)
        {
            await this.client.LinkedServices.CreateOrUpdateAsync(
                this.configuration.ResourceGroup,
                this.configuration.FactoryName,
                name,
                new LinkedServiceResource(new LinkedService(additionalProperties:properties),
                    type:type));
        }

        /// <summary>
        /// Creates the or update dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns></returns>
        public async Task CreateOrUpdateDatasetAsync(Model.DatasetEntity dataset)
        {
            await this.client.Datasets.CreateOrUpdateAsync(
                this.configuration.ResourceGroup,
                this.configuration.FactoryName,
                dataset.Name,
                new DatasetResource(
                    new Dataset(new LinkedServiceReference(dataset.LinkedServiceName, dataset.Properties), description: dataset.Description)));
        }

        /// <summary>
        /// Creates the or update pipeliney.
        /// </summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        public async Task CreateOrUpdatePipelineyAsync(Model.DataPipelineEntity pipeline)
        {
            await this.client.Pipelines.CreateOrUpdateAsync(
                this.configuration.ResourceGroup,
                this.configuration.FactoryName,
                pipeline.Name,
                new PipelineResource(activities: pipeline.ToActivities().ToList(), 
                    description: pipeline.Description, additionalProperties: pipeline.Properties));
        }

        /// <summary>
        /// Runs the pipeline.
        /// </summary>
        /// <param name="pipelineName">Name of the pipeline.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        public async Task<string> RunPipelineAsync(string pipelineName, Dictionary<string, object> properties = null)
        {
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
            var run = await this.client.PipelineRuns.GetAsync(
                this.configuration.ResourceGroup,
                this.configuration.FactoryName,
                runID);

            return run.ToPipelineRunEntity();
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
