namespace ProcessMyMedia.Model
{
    /// <summary>
    /// FtpServer Data Path
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Model.FileSystemDataPath" />
    public class FtpDataPath : FileSystemDataPath
    {
        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        /// <returns></returns>
        public override LinkedServiceType GetServiceType()
        {
            return LinkedServiceType.FtpServer;
        }
    }
}
