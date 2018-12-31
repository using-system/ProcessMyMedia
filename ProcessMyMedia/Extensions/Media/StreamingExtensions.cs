namespace ProcessMyMedia.Extensions
{
    using System.Linq;

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

        /// <summary>
        /// Converts to entity.
        /// </summary>
        /// <param name="liveEvent">The live event.</param>
        /// <param name="outputName">Name of the output.</param>
        /// <returns></returns>
        public static Model.LiveEventEntity ToEntity(this LiveEvent liveEvent, string outputName = null)
        {
            if (liveEvent == null)
            {
                return null;
            }

            return new Model.LiveEventEntity()
            {
                LiveEventName = liveEvent.Name,
                LiveOutputName = outputName,
                IngestUrls = liveEvent.Input.Endpoints.Select(endpoint => endpoint.Url).ToList(),
                PreviewUrls = liveEvent.Preview.Endpoints.Select(preview => preview.Url).ToList()
            };
        }
    }
}
