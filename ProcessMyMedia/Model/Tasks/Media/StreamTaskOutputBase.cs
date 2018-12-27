namespace ProcessMyMedia.Model
{
    using System.Collections.Generic;

    public abstract class StreamTaskOutputBase
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
