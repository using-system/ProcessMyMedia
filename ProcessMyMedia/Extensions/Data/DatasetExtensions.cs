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
                Type = source.GetServiceType().ToDatasetType(),
                TypeProperties = source.GetPathProperties()
            };
        }

        /// <summary>
        /// Converts to datasettype.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">DataPathType {source}</exception>
        public static string ToDatasetType(this Model.LinkedServiceType source)
        {
            switch (source)
            {
                case Model.LinkedServiceType.AzureBlobStorage:
                    return "AzureBlob";
                case Model.LinkedServiceType.FileServer:
                case Model.LinkedServiceType.FtpServer:
                case Model.LinkedServiceType.Sftp:
                case Model.LinkedServiceType.Hdfs:
                    return "FileShare";
                case Model.LinkedServiceType.CosmosDb:
                    return "DocumentDbCollection";
                case Model.LinkedServiceType.AzureMySql:
                    return "AzureMySqlTable";
                case Model.LinkedServiceType.AzureSqlDatabase:
                    return "AzureSqlTable";
                case Model.LinkedServiceType.AzureTableStorage:
                    return "AzureTable";
                case Model.LinkedServiceType.AzurePostgreSql:
                    return "AzurePostgreSqlTable";
                case Model.LinkedServiceType.MongoDb:
                    return "MongoDbCollection";
                case Model.LinkedServiceType.Cassandra:
                    return "CassandraTable";
                case Model.LinkedServiceType.Couchbase:
                    return "CouchbaseTable";
                case Model.LinkedServiceType.AmazonS3:
                    return "AmazonS3Object";
                case Model.LinkedServiceType.HttpServer:
                    return "HttpFile";
                case Model.LinkedServiceType.Odbc:
                case Model.LinkedServiceType.Sybase:
                case Model.LinkedServiceType.MySql:
                case Model.LinkedServiceType.PostgreSql:
                case Model.LinkedServiceType.Db2:
                    return "RelationalTable";
                case Model.LinkedServiceType.OData:
                    return "ODataResource";
                case Model.LinkedServiceType.SqlServer:
                    return "SqlServerTable";
                case Model.LinkedServiceType.Oracle:
                    return "OracleTable";
                case Model.LinkedServiceType.Salesforce:
                    return "SalesforceObject";
                default:
                    throw new NotImplementedException($"DataPathType {source} is not supported as an Data Factory Dataset");
            }
        }
    }
}
