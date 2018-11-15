namespace ProcessMyMedia.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Azure.Management.DataFactory.Models;

    /// <summary>
    /// Pipeline extension methods
    /// </summary>
    public static class PipelineExtensions
    {
        /// <summary>
        /// To the dataset entity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Model.DatasetEntity ToDatasetEntity(this DatasetResource source)
        {
            return new Model.DatasetEntity()
            {
                Name = source.Name,
                Description = source?.Properties?.Description,
                LinkedServiceName = source?.Properties?.LinkedServiceName?.ReferenceName,
                Properties = source?.Properties?.AdditionalProperties.ToDictionary(kv => kv.Key, kv => kv.Value)
            };
        }

        /// <summary>
    /// To the activities.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    public static IEnumerable<Activity> ToActivities(this Model.DataPipelineEntity source)
        {
            return source.Activities.Select(activity => activity.ToActivity());
        }

        /// <summary>
        /// To the activity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Activity ToActivity(this Model.DataActivityEntity source)
        {
            return new Activity()
            {
                Name = source.Name,
                Description = source.Description,
                AdditionalProperties = source.Properties
            };
        }

        /// <summary>
        /// To the pipeline run entity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="activities">The activities.</param>
        /// <returns></returns>
        public static Model.DataPipelineRunEntity ToPipelineRunEntity(this PipelineRun source, IEnumerable<ActivityRun> activities)
        {
            if (source == null)
            {
                return null;
            }

            var run = new Model.DataPipelineRunEntity()
            {
                ID = source.RunId,
                PipelineName = source.PipelineName,
                StartDate = source.RunStart,
                EndDate = source.RunEnd
            };

            //https://docs.microsoft.com/en-us/azure/data-factory/monitor-programmatically
            if (source.Status == "Succeeded")
            {
                run.IsFinished = true;
            }
            else if(source.RunEnd.HasValue)
            {
                run.OnError = true;
                run.ErrorMessage = activities.FirstOrDefault()?.Error?.ToString();
            }

            return run;
        }
    }
}
