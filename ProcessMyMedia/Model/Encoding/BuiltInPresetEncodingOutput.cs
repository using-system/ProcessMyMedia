namespace ProcessMyMedia.Model
{
    /// <summary>
    /// BuiltInPreset Encoding Output
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Model.EncodingOutputBase" />
    public class BuiltInPresetEncodingOutput : EncodingOutputBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuiltInPresetEncodingOutput"/> class.
        /// </summary>
        /// <param name="preset">The preset.</param>
        public BuiltInPresetEncodingOutput(BuiltInPreset preset)
        {
            this.Preset = preset;
        }

        /// <summary>
        /// Gets the preset.
        /// </summary>
        /// <value>
        /// The preset.
        /// </value>
        public BuiltInPreset Preset { get; private set; }
    }
}
