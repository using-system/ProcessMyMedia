namespace ProcessMyMedia.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Live Event Entity
    /// </summary>
    public class LiveEventEntity
    {
        /// <summary>
        /// Gets or sets the ingest urls.
        /// </summary>
        /// <value>
        /// The ingest urls.
        /// </value>
        public List<string> IngestUrls { get; set; }

        /// <summary>
        /// Gets or sets the preview urls.
        /// </summary>
        /// <value>
        /// The preview urls.
        /// </value>
        public List<string> PreviewUrls { get; set; }
    }
}
