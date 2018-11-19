namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Data Path entity
    /// </summary>
    public abstract class DataPath
    {
        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <returns></returns>
        public abstract DataPathType GetDataType();

        /// <summary>
        /// Gets or sets the name of the linked service.
        /// </summary>
        /// <value>
        /// The name of the linked service.
        /// </value>
        public string LinkedServiceName { get; set; }
    }
}
