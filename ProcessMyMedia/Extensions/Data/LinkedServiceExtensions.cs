namespace ProcessMyMedia.Extensions
{
    using Microsoft.Azure.Management.DataFactory.Models;

    /// <summary>
    /// Linked Service Extension methids
    /// </summary>
    public static class LinkedServiceExtensions
    {
        /// <summary>
        /// To the entity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static  Model.LinkedServiceEntity ToEntity(this LinkedServiceResource source)
        {
            if (source == null)
            {
                return null;
            }

            return new Model.LinkedServiceEntity()
            {
                Name = source.Name,
                Type = source.Properties.GetLinkedServiceType().ToString(),
                Description = source.Properties.Description
            };
        }

        /// <summary>
        /// Gets the type of the linked service.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Model.LinkedServiceType GetLinkedServiceType(this LinkedService source)
        {
            if (source == null)
            {
                return Model.LinkedServiceType.Unknown;
            }

            if (source is FtpServerLinkedService)
            {
                return Model.LinkedServiceType.FtpServer;
            }
            else if (source is FileServerLinkedService)
            {
                return Model.LinkedServiceType.FileServer;
            }
            else if (source is HttpLinkedService)
            {
                return Model.LinkedServiceType.HttpServer;
            }
            else if (source is AzureBlobStorageLinkedService)
            {
                return Model.LinkedServiceType.AzureBlobStorage;
            }
            else if (source is AmazonS3LinkedService)
            {
                return Model.LinkedServiceType.AmazonS3;
            }
            else if (source is HdfsLinkedService)
            {
                return Model.LinkedServiceType.Hdfs;
            }
            else if (source is SftpServerLinkedService)
            {
                return Model.LinkedServiceType.Sftp;
            }
            else if (source is AzureTableStorageLinkedService)
            {
                return Model.LinkedServiceType.AzureTableStorage;
            }
            else if (source is MongoDbLinkedService)
            {
                return Model.LinkedServiceType.MongoDb;
            }
            else if (source is ODataLinkedService)
            {
                return Model.LinkedServiceType.OData;
            }
            else if (source is OracleLinkedService)
            {
                return Model.LinkedServiceType.Oracle;
            }
            else if (source is SqlServerLinkedService)
            {
                return Model.LinkedServiceType.SqlServer;
            }
            else if (source is AzureMySqlLinkedService)
            {
                return Model.LinkedServiceType.AzureMySql;
            }
            else if (source is AzureSqlDatabaseLinkedService)
            {
                return Model.LinkedServiceType.AzureSqlDatabase;
            }
            else if (source is AzurePostgreSqlLinkedService)
            {
                return Model.LinkedServiceType.AzurePostgreSql;
            }
            else if (source is SalesforceLinkedService)
            {
                return Model.LinkedServiceType.Salesforce;
            }
            else if (source is Db2LinkedService)
            {
                return Model.LinkedServiceType.Db2;
            }
            else if (source is CosmosDbLinkedService)
            {
                return Model.LinkedServiceType.CosmosDb;
            }
            else if (source is CouchbaseLinkedService)
            {
                return Model.LinkedServiceType.Couchbase;
            }
            else if (source is CassandraLinkedService)
            {
                return Model.LinkedServiceType.Cassandra;
            }
            else if (source is OdbcLinkedService)
            {
                return Model.LinkedServiceType.Odbc;
            }

            return Model.LinkedServiceType.Unknown;
        }
    }
}
