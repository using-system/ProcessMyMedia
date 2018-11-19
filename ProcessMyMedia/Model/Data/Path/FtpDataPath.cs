namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Ftp Data Path
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Model.FileSystemDataPath" />
    public class FtpDataPath : FileSystemDataPath
    {
        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <returns></returns>
        public override DataPathType GetDataType()
        {
            return DataPathType.Ftp;
        }
    }
}
