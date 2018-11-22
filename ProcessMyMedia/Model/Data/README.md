# LinkedServiceEntity

Class to use for the Task CreateLinkedServiceTask to create a new Linked Service.

<table>
 <tr>
  <th>Property Name</th>
  <th>Type</th>
  <th>Description</th>
 </tr>
 <tr>
  <td>Name</td>
  <td>string</td>
  <td>Name of the Azure Data Factory Linked Service</td>
 </tr>
 <tr>
  <td>Type</td>
  <td>string</td>
  <td>Type of the Linked service. Possible values : AmazonMWS, AmazonRedshift, AmazonS3, AzureBatch, AzureBlobStorage, AzureDatabricks, AzureDataLakeAnalytics, AzureDataLakeStore, AzureML, AzureMySql, AzurePostgreSql, AzureSqlDatabase, AzureSqlDW, AzureStorage, AzureTableStorage, Cassandra, Concur, CosmosDb, Couchbase, Db2, Drill, Eloqua, FtpServer, FileServer, GoogleBigQuery, Greenplum, HBase, HDInsight, HDInsightOnDemand, Hive, HttpServer, Hubspot, Impala, Jira, Magento, MariaDB, Marketo, MongoDb, MySql, Netezza, SapBW, SapCloudForCustomer, SalesforceMarketingCloud, Salesforce, QuickBooks, Presto, SapEcc, SapHana, ServiceNow, Sftp, Shopify, Spark, SqlServer, Square, Sybase, Teradata, Vertica, Web, Xero, Zoho</td>
 </tr>
 <tr>
  <td>Description</td>
  <td>string</td>
  <td>Description of the Linked service</td>
 </tr>
 <tr>
  <td>TypeProperties</td>
  <td>object</td>
  <td>The type properties are different for each data store or compute. More details : https://docs.microsoft.com/en-us/rest/api/datafactory/linkedservices/createorupdate</td>
 </tr>
</table>


# GenericDataPath
