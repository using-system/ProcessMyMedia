namespace ProcessMyMedia.Model
{
    public class AacAudioCodec : CodecEntityBase
    {
        /// <summary>
        ///  Gets or sets the encoding profile to be used when encoding audio with AAC. Possible
        //    values include: 'AacLc', 'HeAacV1', 'HeAacV2'
        /// </summary>
        /// <value>
        /// The profile.
        /// </value>
        public string Profile { get; set; }

        /// <summary>
        /// Gets or sets the number of channels in the audio.
        /// </summary>
        /// <value>
        /// The channels.
        /// </value>
        public int? Channels { get; set; }

        /// <summary>
        /// Gets or sets the sampling rate to use for encoding in hertz.
        /// </summary>
        /// <value>
        /// The sampling rate.
        /// </value>
        public int? SamplingRate { get; set; }
   
        /// <summary>
        /// Gets or sets the bitrate, in bits per second, of the output encoded audio.
        /// </summary>
        /// <value>
        /// The bitrate.
        /// </value>
        public int? Bitrate { get; set; }
    }
}
