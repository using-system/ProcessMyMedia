namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Azure Blob Data Path
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Model.FileSystemDataPath" />
    public class AzureBlobDataPath : FileSystemDataPath
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override DataPathType Type => DataPathType.AzureBlobStorage;
    }
}
