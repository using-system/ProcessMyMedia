namespace ProcessMyMedia.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// H264 Video Codec
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Model.CodecEntityBase" />
    public class H264VideoCodec : CodecEntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="H264VideoCodec"/> class.
        /// </summary>
        public H264VideoCodec()
        {
            this.Layers = new List<H264VideoLayer>();
        }

        /// <summary>
        /// Gets or sets the key frame interval.
        /// </summary>
        /// <value>
        /// The key frame interval.
        /// </value>
        public string KeyFrameInterval { get; set; }

        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        /// <value>
        /// The layers.
        /// </value>
        public List<H264VideoLayer> Layers { get; set; }

    }
}
