namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Copy Activity Entity
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Model.DataActivityEntityBase" />
    public class CopyActivityEntity : DataActivityEntityBase
    {
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public DataPath Source { get; set; }

        /// <summary>
        /// Gets or sets the destination.
        /// </summary>
        /// <value>
        /// The destination.
        /// </value>
        public DataPath Destination { get; set; }
    }
}
