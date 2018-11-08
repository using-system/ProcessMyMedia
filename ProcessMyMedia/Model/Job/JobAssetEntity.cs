namespace ProcessMyMedia.Model
{
    using System;

    /// <summary>
    /// Job Asset Entity
    /// </summary>
    public class JobAssetEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public string Label { get; set; }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// </exception>
        public virtual void Validate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new ArgumentException($"{nameof(this.Name)} for Input definition is required");
            }

            if (string.IsNullOrEmpty(this.Label))
            {
                throw new ArgumentException($"{nameof(this.Label)} for Input definition is required");
            }
        }
    }
}
