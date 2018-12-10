namespace ProcessMyMedia.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Stream Task Output
    /// </summary>
    public class StreamTaskOutput
    {
        /// <summary>
        /// Gets or sets the name of the locator.
        /// </summary>
        /// <value>
        /// The name of the locator.
        /// </value>
        public string LocatorName { get; set; }

        /// <summary>
        /// Gets or sets the streaming urls.
        /// </summary>
        /// <value>
        /// The streaming urls.
        /// </value>
        public List<string> StreamingUrls { get; set; }
    }
}
