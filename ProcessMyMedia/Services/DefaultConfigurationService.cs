namespace ProcessMyMedia.Services
{
    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;


    public sealed class DefaultConfigurationService : IConfigurationService
    {
        private MediaConfiguration configuration;

        public MediaConfiguration Configuration => this.configuration;

        public void Initialize(MediaConfiguration configuration)
        {
            this.configuration = configuration;
        }
    }
}
