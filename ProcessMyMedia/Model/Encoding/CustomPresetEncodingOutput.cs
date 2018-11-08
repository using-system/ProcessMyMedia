namespace ProcessMyMedia.Model
{
    using System;
    using System.Collections.Generic;

    public class CustomPresetEncodingOutput : EncodingOutputBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPresetEncodingOutput"/> class.
        /// </summary>
        public CustomPresetEncodingOutput()
        {
            this.Codecs = new List<CodecEntityBase>();
        }

        /// <summary>
        /// Gets or sets the name of the preset.
        /// </summary>
        /// <value>
        /// The name of the preset.
        /// </value>
        public string PresetName { get; set; }

        /// <summary>
        /// Gets or sets the codecs.
        /// </summary>
        /// <value>
        /// The codecs.
        /// </value>
        public List<CodecEntityBase> Codecs { get; set; }


        public override string Label => this.PresetName;

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void Validate()
        {
            if(string.IsNullOrEmpty(this.PresetName))
            {
                throw new ArgumentException($"{nameof(this.PresetName)} for Output definition is required");
            }

            if(this.Codecs.Count == 0)
            {
                throw new ArgumentException($"{nameof(this.Codecs)} for Output definition is empty");
            }
        }
    }
}
