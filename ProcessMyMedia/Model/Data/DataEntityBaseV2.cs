namespace ProcessMyMedia.Model
{
    public class DataEntityBaseV2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataEntityBase"/> class.
        /// </summary>
        public DataEntityBaseV2()
        {

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
        /// Gets or sets the type properties.
        /// </summary>
        /// <value>
        /// The type properties.
        /// </value>
        public object TypeProperties { get; set; }
    }
}
