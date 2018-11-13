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
        /// Creates the or update linked service.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        Task CreateOrUpdateLinkedServiceAsync(string name, string type, Dictionary<string, object> properties);

        /// <summary>
        /// Creates the or update dataset.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="linkedServiceName">Name of the linked service.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        Task CreateOrUpdateDatasetAsync(string name, string linkedServiceName, Dictionary<string, object> properties);

        /// <summary>
        /// Creates the or update pipeliney.
        /// </summary>
        /// <param name="pipeline">The pipeline.</param>
        /// <returns></returns>
        Task CreateOrUpdatePipelineyAsync(Model.DataPipelineEntity pipeline);
    }
}
