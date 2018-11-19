namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Data Path Type
    /// </summary>
    public enum DataPathType
    {
        /// <summary>
        /// The FTP
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-ftp
        /// </summary>
        Ftp,
        /// <summary>
        /// The file system
        /// </summary>
        FileSystem,
        /// <summary>
        /// The azure BLOB storage
        /// https://docs.microsoft.com/en-us/azure/data-factory/connector-azure-blob-storage
        /// </summary>
        AzureBlobStorage
    }
}
