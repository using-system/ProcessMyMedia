namespace ProcessMyMedia
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Middleware Extensions
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Uses the media services.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseMediaServices(this IApplicationBuilder app,
            MediaConfiguration configuration)
        {
            app.ApplicationServices.GetService<IConfigurationService>().Initialize(configuration);

            return app;
        }
    }
}
