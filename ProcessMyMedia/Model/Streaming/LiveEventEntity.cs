namespace ProcessMyMedia.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Live Event Entity
    /// </summary>
    public class LiveEventEntity
    {
        /// <summary>
        /// Gets or sets the name of the live event.
        /// </summary>
        /// <value>
        /// The name of the live event.
        /// </value>
        public string LiveEventName { get; set; }

        /// <summary>
        /// Gets or sets the name of the live output.
        /// </summary>
        /// <value>
        /// The name of the live output.
        /// </value>
        public string LiveOutputName { get; set; }

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
