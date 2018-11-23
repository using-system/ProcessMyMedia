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
                case Model.DataPathType.FileServer:
                case Model.DataPathType.Ftp:
                case Model.DataPathType.AmazonS3:
                case Model.DataPathType.Sftp:
                    return "FileSystemSource";
                case Model.DataPathType.CosmosDb:
                    return "DocumentDbCollectionSource";
                case Model.DataPathType.AzureMySql:
                    return "AzureMySqlSource";
                case Model.DataPathType.AzurePostgreSql:
                    return "AzurePostgreSqlSource";
                case Model.DataPathType.AzureSqlDatabase:
                case Model.DataPathType.SqlServer:
                    return "SqlSource";
                case Model.DataPathType.AzureTableStorage:
                    return "AzureTableSource";
                case Model.DataPathType.MongoDb:
                    return "MongoDbSource";
                case Model.DataPathType.Cassandra:
                    return "CassandraSource";
                case Model.DataPathType.Couchbase:
                    return "CouchbaseSource";
                case Model.DataPathType.Hdfs:
                    return "HdfsSource";
                case Model.DataPathType.HttpServer:
                    return "HttpSource";
                case Model.DataPathType.Odbc:
                case Model.DataPathType.OData:
                case Model.DataPathType.Sybase:
                case Model.DataPathType.MySql:
                case Model.DataPathType.PostgreSql:
                case Model.DataPathType.Db2:
                    return "RelationalSource";
                case Model.DataPathType.Oracle:
                    return "OracleSource";
                case Model.DataPathType.Salesforce:
                    return "SalesforceSource";
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
                case Model.DataPathType.FileServer:
                case Model.DataPathType.Ftp:
                    return "FileSystemSink";
                case Model.DataPathType.CosmosDb:
                    return "DocumentDbCollectionSink";
                case Model.DataPathType.AzureSqlDatabase:
                case Model.DataPathType.SqlServer:
                    return "SqlSink";
                case Model.DataPathType.AzureTableStorage:
                    return "AzureTableSink";
                case Model.DataPathType.Odbc:
                    return "OdbcSink";
                case Model.DataPathType.Oracle:
                    return "OracleSink";
                case Model.DataPathType.Salesforce:
                    return "SalesforceSink";
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
            sourceProperties.Add(new JProperty("type", source.Source.GetDataType().ToSourceType()));
            foreach (var property in source.Source.GetActivityProperties())
            {
                sourceProperties.Add(property);
            }

            JObject destinationProperties = new JObject();
            destinationProperties.Add(new JProperty("type", source.Destination.GetDataType().ToSinkType()));
            foreach (var property in source.Destination.GetActivityProperties())
            {
                destinationProperties.Add(property);
            }


            typedProperties.Add("source", sourceProperties);
            typedProperties.Add("sink", destinationProperties);

            return typedProperties;
        }
    }
}
