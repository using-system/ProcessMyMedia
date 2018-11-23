namespace ProcessMyMedia.Model
{
    public class GenericDataPath : DataPath
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public LinkedServiceType Type { get; set; }

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <returns></returns>
        public override LinkedServiceType GetServiceType()
        {
            return this.Type;
        }

        /// <summary>
        /// Gets or sets the path properties.
        /// </summary>
        /// <value>
        /// The path properties.
        /// </value>
        public object PathProperties { get; set; }

        /// <summary>
        /// Gets or sets the activity properties.
        /// </summary>
        /// <value>
        /// The activity properties.
        /// </value>
        public object ActivityProperties { get; set; }
    }
}
