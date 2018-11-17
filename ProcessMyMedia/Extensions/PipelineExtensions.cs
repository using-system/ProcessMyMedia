namespace ProcessMyMedia.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Azure.Management.DataFactory.Models;
    using Newtonsoft.Json.Linq;

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
            var dataset = new Model.DatasetEntity()
            {
                Name = source.Name,
                Description = source?.Properties?.Description,
                LinkedServiceName = source?.Properties?.LinkedServiceName?.ReferenceName,
            };

            if (source?.Properties?.AdditionalProperties?.ContainsKey("typeProperties") == true)
            {
                dataset.TypeProperties = source.Properties.AdditionalProperties["typeProperties"];
            }

            return dataset;
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
                AdditionalProperties = new Dictionary<string, object>()
                {
                    {"type", source.Type},
                    {
                        "inputs",
                        JArray.FromObject(new[]
                            {new {referenceName = source.InputDatasetName, type = "DatasetReference"}})
                    },
                    {
                        "outputs",
                        JArray.FromObject(new[]
                            {new {referenceName = source.OutputDatasetName, type = "DatasetReference"}})
                    },
                    {"typeProperties", JObject.FromObject(source.TypeProperties)}
                }
            };
        }

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
                run.ErrorMessage = activities.FirstOrDefault()?.Error?.ToString();
            }

            return run;
        }
    }
}
