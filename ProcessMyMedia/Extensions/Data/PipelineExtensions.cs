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
        /// To the pipeline entity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Model.DataPipelineEntity ToPipelineEntity(this PipelineResource source)
        {
            if (source == null)
            {
                return null;
            }

            var target = new Model.DataPipelineEntity()
            {
                Name = source.Name,
                Description = source.Description,
            };

            if (source?.AdditionalProperties?.ContainsKey("typeProperties") == true)
            {
                target.TypeProperties = source.AdditionalProperties["typeProperties"];
            }

            return target;
        }

        /// <summary>
        /// To the pipeline run entity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="activities">The activities.</param>
        /// <returns></returns>
        public static Model.DataPipelineRunEntity ToPipelineRunEntity(this PipelineRun source,
            IEnumerable<ActivityRun> activities)
        {
            var firstActivity = activities.FirstOrDefault();

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
            else if (source.RunEnd.HasValue)
            {
                run.IsFinished = true;
                run.OnError = true;
                run.ErrorMessage = firstActivity?.Error?.ToString();
            }

            return run;
        }
    }
}
