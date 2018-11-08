namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Job Output Entity
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Model.JobAssetEntity" />
    public class JobOutputEntity : JobAssetEntity
    {
        /// <summary>
        /// Gets or sets the progress.
        /// </summary>
        /// <value>
        /// The progress.
        /// </value>
        public int Progress { get; set; }
    }
}
