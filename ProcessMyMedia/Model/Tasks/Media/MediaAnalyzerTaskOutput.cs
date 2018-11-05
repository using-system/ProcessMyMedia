namespace ProcessMyMedia.Model
{
    /// <summary>
    /// MediaAnalyzer Task Output
    /// </summary>
    public class MediaAnalyzerTaskOutput
    {
        /// <summary>
        /// Gets or sets the job.
        /// </summary>
        /// <value>
        /// The job.
        /// </value>
        public JobEntity Job { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public AnalyzingResult Result { get; set; }
    }
}
