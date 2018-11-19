namespace ProcessMyMedia.Model
{
    using Newtonsoft.Json.Linq;

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

        /// <summary>
        /// Gets the path properties.
        /// </summary>
        /// <returns></returns>
        public virtual IJEnumerable<JProperty> GetPathProperties()
        {
            return null;
        }

        /// <summary>
        /// Gets the copy properties.
        /// </summary>
        /// <returns></returns>
        public virtual IJEnumerable<JProperty> GetCopyProperties()
        {
            return null;
        }
    }
}
