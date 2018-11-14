namespace ProcessMyMedia.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Data Entity Base class
    /// </summary>
    public abstract class DataEntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntityBase"/> class.
        /// </summary>
        public DataEntityBase()
        {
            this.Properties = new Dictionary<string, object>();
        }

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
