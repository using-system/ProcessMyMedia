namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Azure Data Factory Configuration
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Model.AzureConfiguration" />
    public class AdfConfiguration : AzureConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the factory.
        /// </summary>
        /// <value>
        /// The name of the factory.
        /// </value>
        public string FactoryName { get; set; }
    }
}
