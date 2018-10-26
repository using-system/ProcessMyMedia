namespace ProcessMyMedia.Services.Contract
{
    using ProcessMyMedia.Model;

    /// <summary>
    /// Configuration Service contract
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Initializes the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        void Initialize(MediaConfiguration configuration);

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        MediaConfiguration Configuration { get; }
    }
}
