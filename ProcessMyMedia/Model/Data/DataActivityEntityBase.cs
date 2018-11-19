namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Data Activity Entity Base class
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Model.DataEntityBase" />
    public abstract class DataActivityEntityBase : DataEntityBase
    {
        /// <summary>
        /// Gets or sets the name of the input dataset.
        /// </summary>
        /// <value>
        /// The name of the input dataset.
        /// </value>
        public string InputDatasetName { get; set; }

        /// <summary>
        /// Gets or sets the name of the output dataset.
        /// </summary>
        /// <value>
        /// The name of the output dataset.
        /// </value>
        public string OutputDatasetName { get; set; }
    }
}
