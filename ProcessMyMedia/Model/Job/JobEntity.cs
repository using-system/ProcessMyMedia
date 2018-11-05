namespace ProcessMyMedia.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Job Entity
    /// </summary>
    public class JobEntity
    {
        /// <summary>
        /// Gets or sets the job identifier.
        /// </summary>
        /// <value>
        /// The job identifier.
        /// </value>
        public string JobID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is finished.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is finished; otherwise, <c>false</c>.
        /// </value>
        public bool IsFinished { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [on error].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [on error]; otherwise, <c>false</c>.
        /// </value>
        public bool OnError { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="JobEntity"/> is canceled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if canceled; otherwise, <c>false</c>.
        /// </value>
        public bool Canceled { get; set; }

        /// <summary>
        /// Gets or sets the input asset names.
        /// </summary>
        /// <value>
        /// The input asset names.
        /// </value>
        public IEnumerable<string> InputAssetNames { get; set; }

        /// <summary>
        /// Gets or sets the output asset names.
        /// </summary>
        /// <value>
        /// The output asset names.
        /// </value>
        public IEnumerable<string> OutputAssetNames { get; set; }
    }
}
