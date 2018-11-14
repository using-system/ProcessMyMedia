namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Dataset Entity
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Model.DataEntityBase" />
    public class DatasetEntity : DataEntityBase
    {
        /// <summary>
        /// Gets or sets the name of the linked service.
        /// </summary>
        /// <value>
        /// The name of the linked service.
        /// </value>
        public string LinkedServiceName { get; set; }
    }
}
