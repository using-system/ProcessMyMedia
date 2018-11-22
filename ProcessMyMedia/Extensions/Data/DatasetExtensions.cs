namespace ProcessMyMedia.Extensions
{
    using System;

    using Microsoft.Azure.Management.DataFactory.Models;

    /// <summary>
    /// Dataset extension methods
    /// </summary>
    public static class DatasetExtensions
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
        /// Converts to datasetentity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Model.DatasetEntity ToDatasetEntity(this Model.DataPath source)
        {
            return new Model.DatasetEntity()
            {
                Name = Guid.NewGuid().ToString(),
                LinkedServiceName = source.LinkedServiceName,
                Type = source.GetDataType().ToDatasetType(),
                TypeProperties = source.GetPathProperties()
            };
        }

        /// <summary>
        /// Converts to datasettype.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">DataPathType {source}</exception>
        public static string ToDatasetType(this Model.DataPathType source)
        {
            switch (source)
            {
                case Model.DataPathType.AzureBlobStorage:
                    return "AzureBlob";
                case Model.DataPathType.FileSystem:
                case Model.DataPathType.Ftp:
                    return "FileShare";
                default:
                    throw new NotImplementedException($"DataPathType {source} is not supported as an Data Factory Dataset");
            }
        }
    }
}
