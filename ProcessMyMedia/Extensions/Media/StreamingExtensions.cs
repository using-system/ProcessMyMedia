namespace ProcessMyMedia.Extensions
{
    using Microsoft.Azure.Management.Media;
    using Microsoft.Azure.Management.Media.Models;

    /// <summary>
    /// Streaming Extension methods
    /// </summary>
    public static class StreamingExtensions
    {
        /// <summary>
        /// To the streaming locator.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="assetName">Name of the asset.</param>
        /// <returns></returns>
        public static StreamingLocator ToStreamingLocator(this Model.StreamingOptions options, string assetName)
        {
            if (options == null)
            {
                return new StreamingLocator()
                {
                    AssetName = assetName,
                    StreamingPolicyName = PredefinedStreamingPolicy.ClearStreamingOnly
                };
            }

            return new StreamingLocator()
            {
                AssetName = assetName,
                StartTime = options.StartDate,
                EndTime = options.EndDate,
                StreamingPolicyName = PredefinedStreamingPolicy.ClearStreamingOnly
            };
        }
    }
}
