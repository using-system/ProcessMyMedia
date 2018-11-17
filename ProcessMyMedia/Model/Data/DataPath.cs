namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Data Path entity
    /// </summary>
    public class DataPath
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public DataPathType Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the linked service.
        /// </summary>
        /// <value>
        /// The name of the linked service.
        /// </value>
        public string LinkedServiceName { get; set; }

        /// <summary>
        /// Gets or sets the path properties.
        /// </summary>
        /// <value>
        /// The path properties.
        /// </value>
        public object PathProperties { get; set; }

        /// <summary>
        /// Gets or sets the copy properties.
        /// </summary>
        /// <value>
        /// The copy properties.
        /// </value>
        public object CopyProperties { get; set; }
    }
}
