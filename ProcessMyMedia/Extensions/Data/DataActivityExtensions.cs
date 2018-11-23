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
        public static string ToSourceType(this Model.LinkedServiceType source)
        {
            switch (source)
            {
                case Model.LinkedServiceType.AzureBlobStorage:
                    return "BlobSource";
                case Model.LinkedServiceType.FileServer:
                case Model.LinkedServiceType.FtpServer:
                case Model.LinkedServiceType.AmazonS3:
                case Model.LinkedServiceType.Sftp:
                    return "FileSystemSource";
                case Model.LinkedServiceType.CosmosDb:
                    return "DocumentDbCollectionSource";
                case Model.LinkedServiceType.AzureMySql:
                    return "AzureMySqlSource";
                case Model.LinkedServiceType.AzurePostgreSql:
                    return "AzurePostgreSqlSource";
                case Model.LinkedServiceType.AzureSqlDatabase:
                case Model.LinkedServiceType.SqlServer:
                    return "SqlSource";
                case Model.LinkedServiceType.AzureTableStorage:
                    return "AzureTableSource";
                case Model.LinkedServiceType.MongoDb:
                    return "MongoDbSource";
                case Model.LinkedServiceType.Cassandra:
                    return "CassandraSource";
                case Model.LinkedServiceType.Couchbase:
                    return "CouchbaseSource";
                case Model.LinkedServiceType.Hdfs:
                    return "HdfsSource";
                case Model.LinkedServiceType.HttpServer:
                    return "HttpSource";
                case Model.LinkedServiceType.Odbc:
                case Model.LinkedServiceType.OData:
                case Model.LinkedServiceType.Sybase:
                case Model.LinkedServiceType.MySql:
                case Model.LinkedServiceType.PostgreSql:
                case Model.LinkedServiceType.Db2:
                    return "RelationalSource";
                case Model.LinkedServiceType.Oracle:
                    return "OracleSource";
                case Model.LinkedServiceType.Salesforce:
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
        public static string ToSinkType(this Model.LinkedServiceType source)
        {
            switch (source)
            {
                case Model.LinkedServiceType.AzureBlobStorage:
                    return "BlobSink";
                case Model.LinkedServiceType.FileServer:
                case Model.LinkedServiceType.FtpServer:
                    return "FileSystemSink";
                case Model.LinkedServiceType.CosmosDb:
                    return "DocumentDbCollectionSink";
                case Model.LinkedServiceType.AzureSqlDatabase:
                case Model.LinkedServiceType.SqlServer:
                    return "SqlSink";
                case Model.LinkedServiceType.AzureTableStorage:
                    return "AzureTableSink";
                case Model.LinkedServiceType.Odbc:
                    return "OdbcSink";
                case Model.LinkedServiceType.Oracle:
                    return "OracleSink";
                case Model.LinkedServiceType.Salesforce:
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
            sourceProperties.Add(new JProperty("type", source.Source.GetServiceType().ToSourceType()));
            foreach (var property in source.Source.GetActivityProperties())
            {
                sourceProperties.Add(property);
            }

            JObject destinationProperties = new JObject();
            destinationProperties.Add(new JProperty("type", source.Destination.GetServiceType().ToSinkType()));
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
