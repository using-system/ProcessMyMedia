namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Data Path entity
    /// </summary>
    public abstract class DataPath
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public abstract DataPathType Type { get; }

        /// <summary>
        /// Gets or sets the name of the linked service.
        /// </summary>
        /// <value>
        /// The name of the linked service.
        /// </value>
        public string LinkedServiceName { get; set; }
    }
}
