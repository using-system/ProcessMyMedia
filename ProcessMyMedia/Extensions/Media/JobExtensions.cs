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
                Inputs = source.Input.ToJobInputs().ToList(),
                Outputs = source.Outputs.SelectMany(output => output.ToJobOutputs()).ToList(),
                Canceled = source.State == JobState.Canceled,
                OnError = source.State == JobState.Error,
                IsFinished = source.State == JobState.Canceled 
                             || source.State == JobState.Error
                             || source.State == JobState.Finished,
                Created = source.Created
            };
        }

        /// <summary>
        /// To the job inputs.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IEnumerable<JobInputEntity> ToJobInputs(this JobInput source)
        {
            if (source is JobInputAsset)
            {
                yield return ((JobInputAsset) source).ToJobInput(); 
            }
            else if (source is JobInputs)
            {
                foreach (var input in ((JobInputs) source).Inputs.SelectMany(input => input.ToJobInputs()))
                {
                    yield return input;
                }
            }
        }

        /// <summary>
        /// To the job input.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static JobInputEntity ToJobInput(this JobInputAsset source)
        {
            if (source == null)
            {
                return null;
            }

            return new JobInputEntity()
            {
                Name = source.AssetName,
                Label = source.Label
            };
        }

        /// <summary>
        /// To the asset names.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IEnumerable<JobOutputEntity> ToJobOutputs(this JobOutput source)
        {
            if (source is JobOutputAsset)
            {
                yield return ((JobOutputAsset)source).ToJobOutput();
            }
        }

        /// <summary>
        /// To the job output.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static JobOutputEntity ToJobOutput(this JobOutputAsset source)
        {
            if (source == null)
            {
                return null;
            }

            return new JobOutputEntity()
            {
                Name = source.AssetName,
                Label = source.Label,
                Progress = source.Progress
            };
        }

    }
}
