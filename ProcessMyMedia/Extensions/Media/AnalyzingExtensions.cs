namespace ProcessMyMedia.Extensions
{
    using Microsoft.Azure.Management.Media.Models;

    using ProcessMyMedia.Model;

    /// <summary>
    /// Analyzing Extension methods
    /// </summary>
    public static class AnalyzingExtensions
    {
        /// <summary>
        /// To the analyzer preset.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static VideoAnalyzerPreset ToAnalyzerPreset(this AnalyzingParameters parameters)
        {
            return new VideoAnalyzerPreset(parameters.AudioLanguage, parameters.AnalyzingType.ToInsightsType());
        }

        /// <summary>
        /// To the type of the insights.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static InsightsType ToInsightsType(this AnalyzingType type)
        {
            switch (type)
            {
                case AnalyzingType.AudioOnly:
                    return InsightsType.AudioInsightsOnly;
                case AnalyzingType.VideoOnly:
                    return InsightsType.VideoInsightsOnly;
                default:
                    return InsightsType.AllInsights;
            }
        }
    }
}
