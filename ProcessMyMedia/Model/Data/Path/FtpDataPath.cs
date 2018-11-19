namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Ftp Data Path
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Model.FileSystemDataPath" />
    public class FtpDataPath : FileSystemDataPath
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public override DataPathType Type => DataPathType.Ftp;
    }
}
