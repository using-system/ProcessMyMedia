namespace ProcessMyMedia.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Azure.Management.Media.Models;

    using ProcessMyMedia.Model;

    /// <summary>
    /// Job extension methods
    /// </summary>
    public static class JobExtensions
    {
        /// <summary>
        /// To the job entity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public static JobEntity ToJobEntity(this Job source, string templateName = null)
        {
            if (source == null)
            {
                return null;
            }

            return new JobEntity()
            {
                ID = source.Id,
                Name = source.Name,
                TemplateName = templateName,
                InputAssetNames = source.Input.ToAssetNames(),
                OutputAssetNames = source.Outputs.SelectMany(output => output.ToAssetNames()),
                Canceled = source.State == JobState.Canceled,
                OnError = source.State == JobState.Error,
                IsFinished = source.State == JobState.Canceled 
                             || source.State == JobState.Error
                             || source.State == JobState.Finished,
                Created = source.Created
            };
        }

        /// <summary>
        /// To the asset names.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToAssetNames(this JobInput source)
        {
            if (source is JobInputAsset)
            {
                yield return ((JobInputAsset) source).AssetName; 
            }
            else if (source is JobInputs)
            {
                foreach (var input in ((JobInputs) source).Inputs.SelectMany(input => input.ToAssetNames()))
                {
                    yield return input;
                }
            }
        }

        /// <summary>
        /// To the asset names.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IEnumerable<string> ToAssetNames(this JobOutput source)
        {
            if (source is JobOutputAsset)
            {
                yield return ((JobOutputAsset)source).AssetName;
            }
        }
    }
}
