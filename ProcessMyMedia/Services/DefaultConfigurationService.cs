namespace ProcessMyMedia.Services
{
    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;


    /// <summary>
    /// Default Configuration Service
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Services.Contract.IConfigurationService" />
    public sealed class DefaultConfigurationService : IConfigurationService
    {
        private MediaConfiguration configuration;

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public MediaConfiguration Configuration => this.configuration;

        /// <summary>
        /// Initializes the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void Initialize(MediaConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
