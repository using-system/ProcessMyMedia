namespace ProcessMyMedia.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Dataset
    /// </summary>
    public class Dataset
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public Dictionary<string, string> Properties { get; set; }
    }
}
