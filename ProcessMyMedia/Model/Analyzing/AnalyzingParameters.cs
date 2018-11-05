namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Analyzing Parameters
    /// </summary>
    public class AnalyzingParameters
    {
        /// <summary>
        /// Gets or sets the audio language.
        /// </summary>
        /// <value>
        /// The audio language.
        /// </value>
        public string AudioLanguage { get; set; }

        /// <summary>
        /// Gets or sets the type of the analyzing.
        /// </summary>
        /// <value>
        /// The type of the analyzing.
        /// </value>
        public AnalyzingType AnalyzingType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyzingParameters"/> class.
        /// </summary>
        public AnalyzingParameters()
        {
            this.AnalyzingType = AnalyzingType.All;
        }
    }
}
