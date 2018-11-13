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
        /// Adds the linked service.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        Task AddLinkedServiceAsync(string name, string type, Dictionary<string, object> properties)
    }
}
