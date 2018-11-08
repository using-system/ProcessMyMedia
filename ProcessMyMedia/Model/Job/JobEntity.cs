namespace ProcessMyMedia.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Job Entity
    /// </summary>
    public class JobEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobEntity"/> class.
        /// </summary>
        public JobEntity()
        {
            this.Inputs = new List<JobInputEntity>();
            this.Outputs = new List<JobOutputEntity>();
        }

        /// <summary>
        /// Gets or sets the job identifier.
        /// </summary>
        /// <value>
        /// The job identifier.
        /// </value>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the template.
        /// </summary>
        /// <value>
        /// The name of the template.
        /// </value>
        public string TemplateName { get; set; }

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
        public List<JobInputEntity> Inputs { get; set; }

        /// <summary>
        /// Gets or sets the output asset names.
        /// </summary>
        /// <value>
        /// The output asset names.
        /// </value>
        public List<JobOutputEntity> Outputs { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        public DateTime Created { get; set; }
    }
}
