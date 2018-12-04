namespace ProcessMyMedia.Services.Contract
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Data Factory Service Contract
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Services.Contract.IAzureService" />
    public interface IDataFactoryService : IAzureService
    {
        /// <summary>
        /// Gets the linked service.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Task<Model.LinkedServiceEntity> GetLinkedServiceAsync(string name);

        /// <summary>
        /// Creates the or update linked service.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="typeProperties">The type properties.</param>
        /// <returns></returns>
        Task CreateOrUpdateLinkedServiceAsync(string name, string type, object typeProperties);

        /// <summary>
        /// Gets the dataset.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Task<Model.DatasetEntity> GetDatasetAsync(string name);

        /// <summary>
        /// Creates the or update dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <returns></returns>
        Task<Model.DatasetEntity> CreateOrUpdateDatasetAsync(Model.DatasetEntity dataset);

        /// <summary>
        /// Deletes the dataset.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Task DeleteDatasetAsync(string name);

        /// <summary>
        /// Gets the pipeline.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Task<Model.DataPipelineEntity> GetPipelineAsync(string name);

        /// <summary>
        /// Creates the or update pipeliney.
        /// </summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        Task CreateOrUpdatePipelineyAsync(Model.DataPipelineEntity pipeline);

        /// <summary>
        /// Deletes the pipeline.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Task DeletePipelineAsync(string name);

        /// <summary>
        /// Runs the pipeline.
        /// </summary>
        /// <param name="pipelineName">Name of the pipeline.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        Task<string> RunPipelineAsync(string pipelineName, Dictionary<string, object> properties = null);

        /// <summary>
        /// Getpipelines the run.
        /// </summary>
        /// <param name="runID">The run identifier.</param>
        /// <returns></returns>
        Task<Model.DataPipelineRunEntity> GetPipelineRunAsync(string runID);
    }
}
