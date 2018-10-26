namespace ProcessMyMedia.Services.Contract
{
    using ProcessMyMedia.Model;

    public interface IConfigurationService
    {
        void Initialize(MediaConfiguration configuration);

        MediaConfiguration Configuration { get; }
    }
}
