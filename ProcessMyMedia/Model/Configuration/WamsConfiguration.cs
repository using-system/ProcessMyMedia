namespace ProcessMyMedia.Model
{
    /// <summary>
    /// Media Configuration
    /// </summary>
    public class WamsConfiguration
    {
        /// <summary>
        /// Gets or sets the arm endpoint.
        /// </summary>
        /// <value>
        /// The arm endpoint.
        /// </value>
        public string ArmEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the name of the media account.
        /// </summary>
        /// <value>
        /// The name of the media account.
        /// </value>
        public string MediaAccountName { get; set; }

        /// <summary>
        /// Gets or sets the subscription identifier.
        /// </summary>
        /// <value>
        /// The subscription identifier.
        /// </value>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the resource group.
        /// </summary>
        /// <value>
        /// The resource group.
        /// </value>
        public string ResourceGroup { get; set; }

        /// <summary>
        /// Gets or sets the aad tenant identifier.
        /// </summary>
        /// <value>
        /// The aad tenant identifier.
        /// </value>
        public string AadTenantId { get; set; }

        /// <summary>
        /// Gets or sets the aad client identifier.
        /// </summary>
        /// <value>
        /// The aad client identifier.
        /// </value>
        public string AadClientId { get; set; }

        /// <summary>
        /// Gets or sets the aad secret.
        /// </summary>
        /// <value>
        /// The aad secret.
        /// </value>
        public string AadSecret { get; set; }
    }
}
