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
                case Model.DataPathType.FileServer:
                case Model.DataPathType.Ftp:
                case Model.DataPathType.Sftp:
                case Model.DataPathType.Hdfs:
                    return "FileShare";
                case Model.DataPathType.CosmosDb:
                    return "DocumentDbCollection";
                case Model.DataPathType.AzureMySql:
                    return "AzureMySqlTable";
                case Model.DataPathType.AzureSqlDatabase:
                    return "AzureSqlTable";
                case Model.DataPathType.AzureTableStorage:
                    return "AzureTable";
                case Model.DataPathType.AzurePostgreSql:
                    return "AzurePostgreSqlTable";
                case Model.DataPathType.MongoDb:
                    return "MongoDbCollection";
                case Model.DataPathType.Cassandra:
                    return "CassandraTable";
                case Model.DataPathType.Couchbase:
                    return "CouchbaseTable";
                case Model.DataPathType.AmazonS3:
                    return "AmazonS3Object";
                case Model.DataPathType.HttpServer:
                    return "HttpFile";
                case Model.DataPathType.Odbc:
                case Model.DataPathType.Sybase:
                case Model.DataPathType.MySql:
                case Model.DataPathType.PostgreSql:
                case Model.DataPathType.Db2:
                    return "RelationalTable";
                case Model.DataPathType.OData:
                    return "ODataResource";
                case Model.DataPathType.SqlServer:
                    return "SqlServerTable";
                case Model.DataPathType.Oracle:
                    return "OracleTable";
                case Model.DataPathType.Salesforce:
                    return "SalesforceObject";
                default:
                    throw new NotImplementedException($"DataPathType {source} is not supported as an Data Factory Dataset");
            }
        }
    }
}
