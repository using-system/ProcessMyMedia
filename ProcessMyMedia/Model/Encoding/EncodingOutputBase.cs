namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Encoding output base class
    /// </summary>
    public abstract class EncodingOutputBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public abstract string Label { get; }
    }
}
