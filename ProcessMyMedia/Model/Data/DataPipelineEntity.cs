namespace ProcessMyMedia.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Data Pipeline Entity
    /// </summary>
    public class DataPipelineEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public Dictionary<string, object> Properties { get; set; }
    
    }
}
