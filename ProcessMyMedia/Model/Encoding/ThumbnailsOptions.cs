namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Thumbnails Options
    /// </summary>
    public class ThumbnailsOptions
    {
        /// <summary>
        /// Gets or sets the filename pattern.
        /// </summary>
        /// <value>
        /// The filename pattern.
        /// </value>
        public string FilenamePattern { get; set; }

        /// <summary>
        /// Gets or sets the generate PNG.
        /// </summary>
        /// <value>
        /// The generate PNG.
        /// </value>
        public bool? GeneratePng { get; set; }

        /// <summary>
        /// Gets or sets the generate GIF.
        /// </summary>
        /// <value>
        /// The generate GIF.
        /// </value>
        public bool? GenerateGif { get; set; }

        /// <summary>
        /// Gets or sets the generate JPG.
        /// </summary>
        /// <value>
        /// The generate JPG.
        /// </value>
        public bool? GenerateJpg { get; set; }

  
        ///  Gets or sets the position in the input video from where to start generating thumbnails.
        //     The value can be in absolute timestamp (ISO 8601, e.g: PT05S), or a frame count
        //     (For example, 10 for the 10th frame), or a relative value (For example, 1%).
        //     Also supports a macro {Best}, which tells the encoder to select the best thumbnail
        //     from the first few seconds of the video.  
        /// </summary>
        /// <value>
        /// The start.
        /// </value>
        public string Start { get; set; }
        //
        // Résumé :
        //     Gets or sets the intervals at which thumbnails are generated. The value can be
        //     in absolute timestamp (ISO 8601, e.g: PT05S for one image every 5 seconds), or
        //     a frame count (For example, 30 for every 30 frames), or a relative value (For
        //     example, 1%).
        /// <summary>
        /// Gets or sets the intervals at which thumbnails are generated. The value can be
        //     in absolute timestamp (ISO 8601, e.g: PT05S for one image every 5 seconds), or
        //     a frame count (For example, 30 for every 30 frames), or a relative value (For
        //     example, 1%).
        /// </summary>
        /// <value>
        /// The step.
        /// </value>
        public string Step { get; set; }
  
        /// <summary>
        /// Gets or sets the position in the input video at which to stop generating thumbnails.
        //     The value can be in absolute timestamp (ISO 8601, e.g: PT5M30S to stop at 5 minutes
        //     and 30 seconds), or a frame count (For example, 300 to stop at the 300th frame),
        //     or a relative value (For example, 100%).
        /// </summary>
        /// <value>
        /// The range.
        /// </value>
        public string Range { get; set; }

        /// <summary>
        ///  Gets or sets the width of the output video for this layer. The value can be absolute
        //     (in pixels) or relative (in percentage). For example 50% means the output video
        //     has half as many pixels in width as the input.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public string Width { get; set; }
        
        /// <summary>
        ///  Gets or sets the height of the output video for this layer. The value can be
        //     absolute (in pixels) or relative (in percentage). For example 50% means the output
        //     video has half as many pixels in height as the input.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public string Height { get; set; }
    }
}
