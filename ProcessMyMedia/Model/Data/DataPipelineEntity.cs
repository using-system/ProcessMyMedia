namespace ProcessMyMedia.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Data Pipeline Entity
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Model.DataEntityBase" />
    public class DataPipelineEntity : DataEntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataPipelineEntity"/> class.
        /// </summary>
        public DataPipelineEntity()
        {
            this.Activities = new List<DataActivityEntity>();
        }

        /// <summary>
        /// Gets or sets the activities.
        /// </summary>
        /// <value>
        /// The activities.
        /// </value>
        public List<DataActivityEntity> Activities { get; set; }
    
    }
}
