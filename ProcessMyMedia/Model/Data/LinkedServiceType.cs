namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Data Path Type
    /// https://docs.microsoft.com/en-us/azure/data-factory/copy-activity-overview##supported-data-stores-and-formats
    /// </summary>
    public enum LinkedServiceType
    {
        /// <summary>
        /// The unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// The FTP
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-ftp
        /// </summary>
        FtpServer,
        /// <summary>
        /// The file system
        /// </summary>
        FileServer,
        /// <summary>
        /// The azure BLOB storage
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-azure-blob-storage
        /// </summary>
        AzureBlobStorage,
        /// <summary>
        /// The cosmos database
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-azure-cosmos-db
        /// </summary>
        CosmosDb,
        /// <summary>
        /// The azure my SQL
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-azure-database-for-mysql
        /// </summary>
        AzureMySql,
        /// <summary>
        /// The azure postgre SQL
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-azure-database-for-postgresql
        /// </summary>
        AzurePostgreSql,
        /// <summary>
        /// The azure SQL database
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-azure-sql-database
        /// </summary>
        AzureSqlDatabase,
        /// <summary>
        /// The azure table storage
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-azure-table-storage
        /// </summary>
        AzureTableStorage,
        /// <summary>
        /// The mongo database
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-mongodb
        /// </summary>
        MongoDb,
        /// <summary>
        /// The cassandra
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-cassandra
        /// </summary>
        Cassandra,
        /// <summary>
        /// The couchbase
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-couchbase
        /// </summary>
        Couchbase,
        /// <summary>
        /// The amazon s3
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-amazon-simple-storage-service
        /// </summary>
        AmazonS3,
        /// <summary>
        /// The SFTP
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-sftp
        /// </summary>
        Sftp,
        /// <summary>
        /// The HDFS
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-hdfs
        /// </summary>
        Hdfs,
        /// <summary>
        /// The HTTP server
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-http
        /// </summary>
        HttpServer,
        /// <summary>
        /// The ODBC
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-odbc
        /// </summary>
        Odbc,
        /// <summary>
        /// The o data
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-odata
        /// </summary>
        OData,
        /// <summary>
        /// The SQL server
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-sql-server
        /// </summary>
        SqlServer,
        /// <summary>
        /// The sybase
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-sybase
        /// </summary>
        Sybase,
        /// <summary>
        /// My SQL
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-mysql
        /// </summary>
        MySql,
        /// <summary>
        /// The postgre SQL
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-postgresql
        /// </summary>
        PostgreSql,
        /// <summary>
        /// The oracle
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-oracle
        /// </summary>
        Oracle,
        /// <summary>
        /// The DB2
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-db2
        /// </summary>
        Db2,
        /// <summary>
        /// The salesforce
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-salesforce
        /// </summary>
        Salesforce
    }
}
