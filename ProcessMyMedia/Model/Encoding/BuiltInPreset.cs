namespace ProcessMyMedia.Model
{
    public enum BuiltInPreset
    {
        /// <summary>
        /// The aac good quality audio
        /// Produces a single MP4 file containing only stereo audio encoded at 192 kbps.
        /// </summary>
        AACGoodQualityAudio = 0,
        /// <summary>
        /// The adaptive streaming
        /// Produces a set of GOP aligned MP4 files with H.264 video and stereo AAC audio. Auto-generates a bitrate ladder based on the input resolution and bitrate. The auto-generated preset will never exceed the input resolution and bitrate. For example, if the input is 720p at 3 Mbps, output will remain 720p at best, and will start at rates lower than 3 Mbps. The output will will have video and audio in separate MP4 files, which is optimal for adaptive streaming.
        /// </summary>
        AdaptiveStreaming,
        /// <summary>
        /// The H264 multiple bitrate1080p
        /// Produces a set of 8 GOP-aligned MP4 files, ranging from 6000 kbps to 400 kbps, and stereo AAC audio. Resolution starts at 1080p and goes down to 360p.
        /// </summary>
        H264MultipleBitrate1080p,
        /// <summary>
        /// The H264 multiple bitrate720p
        /// Produces a set of 6 GOP-aligned MP4 files, ranging from 3400 kbps to 400 kbps, and stereo AAC audio. Resolution starts at 720p and goes down to 360p.
        /// </summary>
        H264MultipleBitrate720p,
        /// <summary>
        /// The H264 multiple bitrate sd
        /// Produces a set of 5 GOP-aligned MP4 files, ranging from 1600kbps to 400 kbps, and stereo AAC audio. Resolution starts at 480p and goes down to 360p.
        /// </summary>
        H264MultipleBitrateSD,
        /// <summary>
        /// The H264 single bitrate1080p
        /// Produces an MP4 file where the video is encoded with H.264 codec at 6750 kbps and a picture height of 1080 pixels, and the stereo audio is encoded with AAC-LC codec at 64 kbps.
        /// </summary>
        H264SingleBitrate1080p,
        /// <summary>
        /// The H264 single bitrate720p
        /// Produces an MP4 file where the video is encoded with H.264 codec at 4500 kbps and a picture height of 720 pixels, and the stereo audio is encoded with AAC-LC codec at 64 kbps.
        /// </summary>
        H264SingleBitrate720p,
        /// <summary>
        /// The H264 single bitrate sd
        /// Produces an MP4 file where the video is encoded with H.264 codec at 2200 kbps and a picture height of 480 pixels, and the stereo audio is encoded with AAC-LC codec at 64 kbps.
        /// </summary>
        H264SingleBitrateSD
    }
}
