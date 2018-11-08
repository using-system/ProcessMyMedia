namespace ProcessMyMedia.Model
{
    /// <summary>
    /// H264 Video Layer
    /// </summary>
    public class H264VideoLayer
    {
        /// <summary>
        /// Gets or sets the bitrate.
        /// </summary>
        /// <value>
        /// The bitrate.
        /// </value>
        public int Bitrate { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public string Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public string Height { get; set; }
    }
}
