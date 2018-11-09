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
        public int? Bitrate { get; set; }

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
   
        /// <summary>
        /// Gets or sets the maximum bitrate (in bits per second), at which the VBV buffer
        //     should be assumed to refill. If not specified, defaults to the same value as
        //     bitrate.
        /// </summary>
        /// <value>
        /// The maximum bitrate.
        /// </value>
        public int? MaxBitrate { get; set; }

        /// <summary>
        /// Gets or sets the number of B-frames to be used when encoding this layer. If not
        //     specified, the encoder chooses an appropriate number based on the video profile
        //     and level.
        /// </summary>
        /// <value>
        /// The b frames.
        /// </value>
        public int? BFrames { get; set; }
 
        /// <summary>
        /// Gets or sets the frame rate (in frames per second) at which to encode this layer.
        //     The value can be in the form of M/N where M and N are integers (For example,
        //     30000/1001), or in the form of a number (For example, 30, or 29.97). The encoder
        //     enforces constraints on allowed frame rates based on the profile and level. If
        //     it is not specified, the encoder will use the same frame rate as the input video.
        /// </summary>
        /// <value>
        /// The frame rate.
        /// </value>
        public string FrameRate { get; set; }

        /// <summary>
        ///   Gets or sets the number of slices to be used when encoding this layer. If not
        //     specified, default is zero, which means that encoder will use a single slice
        //     for each frame.
        /// </summary>
        /// <value>
        /// The slices.
        /// </value>
        public int? Slices { get; set; }
     
        /// <summary>
        /// Gets or sets whether or not adaptive B-frames are to be used when encoding this
        //     layer. If not specified, the encoder will turn it on whenever the video profile
        //     permits its use.
        /// </summary>
        /// <value>
        /// The adaptive b frame.
        /// </value>
        public bool? AdaptiveBFrame { get; set; }

        /// <summary>
        ///  Gets or sets which profile of the H.264 standard should be used when encoding
        //     this layer. Default is Auto. Possible values include: 'Auto', 'Baseline', 'Main',
        //     'High', 'High422', 'High444'
        /// </summary>
        /// <value>
        /// The profile.
        /// </value>
        public string Profile { get; set; }

        /// <summary>
        /// Gets or sets which level of the H.264 standard should be used when encoding this
        //     layer. The value can be Auto, or a number that matches the H.264 profile. If
        //     not specified, the default is Auto, which lets the encoder choose the Level that
        //     is appropriate for this layer.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        public string Level { get; set; }
  
        /// <summary>
        /// Gets or sets the VBV buffer window length. The value should be in ISO 8601 format.
        //     The value should be in the range [0.1-100] seconds. The default is 5 seconds
        //     (for example, PT5S).
        /// </summary>
        /// <value>
        /// The buffer window.
        /// </value>
        public string BufferWindow { get; set; }

        /// <summary>
        /// Gets or sets the number of reference frames to be used when encoding this layer.
        //     If not specified, the encoder determines an appropriate number based on the encoder
        //     complexity setting.
        /// </summary>
        /// <value>
        /// The reference frames.
        /// </value>
        public int? ReferenceFrames { get; set; }

        /// <summary>
        ///  Gets or sets the entropy mode to be used for this layer. If not specified, the
        //     encoder chooses the mode that is appropriate for the profile and level. Possible
        //     values include: 'Cabac', 'Cavlc'
        /// </summary>
        /// <value>
        /// The entropy mode.
        /// </value>
        public string EntropyMode { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string Label { get; set; }
    }
}
