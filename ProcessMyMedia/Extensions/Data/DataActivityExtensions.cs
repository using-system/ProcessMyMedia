namespace ProcessMyMedia.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Azure.Management.DataFactory.Models;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Data activity extension methods
    /// </summary>
    public static class DataActivityExtensions
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
        public static Activity ToActivity(this Model.DataActivityEntityBase source)
        {
            //TODO : Implement copy activity logic
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
                    {"typeProperties", source.GetTypedProperties()}
                }
            };
        }

        /// <summary>
        /// Converts to copyactivitycopysourcetype.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">DataPathType {source}</exception>
        public static string ToSourceType(this Model.DataPathType source)
        {
            switch (source)
            {
                case Model.DataPathType.AzureBlobStorage:
                    return "BlobSource";
                case Model.DataPathType.FileSystem:
                case Model.DataPathType.Ftp:
                    return "FileSystemSource";
                default:
                    throw new NotImplementedException($"DataPathType {source} is not supported as source");
            }
        }


        /// <summary>
        /// Converts to copyactivitycopysinktype.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">DataPathType {source}</exception>
        public static string ToSinkType(this Model.DataPathType source)
        {
            switch (source)
            {
                case Model.DataPathType.AzureBlobStorage:
                    return "BlobSink";
                case Model.DataPathType.FileSystem:
                case Model.DataPathType.Ftp:
                    return "FileSystemSink";
                default:
                    throw new NotImplementedException($"DataPathType {source} is not supported as source");
            }
        }

        /// <summary>
        /// Gets the typed properties.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static JObject GetTypedProperties(this Model.DataActivityEntityBase source)
        {
            JObject typedProperties = new JObject();

            if (source == null)
            {
                return null;
            }

            if (source is Model.CopyActivityEntity)
            {
                return ((Model.CopyActivityEntity) source).GetTypedProperties();
            }

            return typedProperties;
        }

        /// <summary>
        /// Gets the typed properties.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static JObject GetTypedProperties(this Model.CopyActivityEntity source)
        {
            JObject typedProperties = new JObject();


            JObject sourceProperties = new JObject();
            sourceProperties.Add(new JProperty("type", source.Source.Type.ToSourceType()));
            foreach (var property in source.Source.GetCopyProperties())
            {
                sourceProperties.Add(property);
            }

            JObject destinationProperties = new JObject();
            destinationProperties.Add(new JProperty("type", source.Destination.Type.ToSinkType()));
            foreach (var property in source.Destination.GetCopyProperties())
            {
                destinationProperties.Add(property);
            }


            typedProperties.Add("source", sourceProperties);
            typedProperties.Add("sink", destinationProperties);

            return typedProperties;
        }
    }
}
