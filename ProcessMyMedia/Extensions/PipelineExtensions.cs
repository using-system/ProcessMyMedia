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
        /// <returns></returns>
        public static Model.DataPipelineRunEntity ToPipelineRunEntity(this PipelineRun source)
        {
            if (source == null)
            {
                return null;
            }

            return new Model.DataPipelineRunEntity()
            {
                RunID = source.RunId
            };
        }
    }
}
