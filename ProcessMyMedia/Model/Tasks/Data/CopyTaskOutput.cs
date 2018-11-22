namespace ProcessMyMedia.Model
{
    /// <summary>
    /// GenericCopyTask DatasetOutput
    /// </summary>
    public class CopyTaskOutput
    {

        /// <summary>
        /// Gets or sets the input dataset.
        /// </summary>
        /// <value>
        /// The input dataset.
        /// </value>
        public DatasetEntity InputDataset { get; set; }

        /// <summary>
        /// Gets or sets the output dataset.
        /// </summary>
        /// <value>
        /// The output dataset.
        /// </value>
        public DatasetEntity OutputDataset { get; set; }
        /// <summary>
        /// Gets or sets the run.
        /// </summary>
        /// <value>
        /// The run.
        /// </value>
        public DataPipelineRunEntity Run { get; set; }
    }
}
