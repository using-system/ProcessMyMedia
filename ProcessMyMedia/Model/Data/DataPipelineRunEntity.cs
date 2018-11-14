namespace ProcessMyMedia.Model
{
    using System;

    public class DataPipelineRunEntity
    {
        /// <summary>
        /// Gets or sets the run identifier.
        /// </summary>
        /// <value>
        /// The run identifier.
        /// </value>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the name of the pipeline.
        /// </summary>
        /// <value>
        /// The name of the pipeline.
        /// </value>
        public string PipelineName { get; set; }

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
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }


        /// <summary>
        /// Gets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime? EndDate { get; set; }

    }
}
