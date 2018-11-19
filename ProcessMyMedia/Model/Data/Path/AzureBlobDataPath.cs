namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Azure Blob Data Path
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Model.FileSystemDataPath" />
    public class AzureBlobDataPath : FileSystemDataPath
    {
        public override DataPathType GetDataType()
        {
            return DataPathType.AzureBlobStorage;
        }
    }
}
