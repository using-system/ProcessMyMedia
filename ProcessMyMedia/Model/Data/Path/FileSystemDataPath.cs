namespace ProcessMyMedia.Model
{
    /// <summary>
    /// File System Data Path
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Model.DataPath" />
    public abstract class FileSystemDataPath : DataPath
    {
        /// <summary>
        /// Gets or sets the folder path.
        /// </summary>
        /// <value>
        /// The folder path.
        /// </value>
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FileSystemDataPath"/> is recursive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if recursive; otherwise, <c>false</c>.
        /// </value>
        public bool? Recursive { get; set; }

        /// <summary>
        /// Gets or sets the preserve hierarchy.
        /// </summary>
        /// <value>
        /// The preserve hierarchy.
        /// </value>
        public bool? PreserveHierarchy { get; set; }
    }
}
