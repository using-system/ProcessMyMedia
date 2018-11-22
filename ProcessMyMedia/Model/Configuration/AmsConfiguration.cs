namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Media Configuration
    /// </summary>
    public class AmsConfiguration : AzureConfiguration
    {
        /// <summary>
        /// Gets or sets the name of the media account.
        /// </summary>
        /// <value>
        /// The name of the media account.
        /// </value>
        public string MediaAccountName { get; set; }
    }
}
