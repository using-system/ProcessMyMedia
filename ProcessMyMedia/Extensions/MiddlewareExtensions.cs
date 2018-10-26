namespace ProcessMyMedia
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services;
    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Middleware Extensions
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Adds the media services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddMediaServices(this IServiceCollection services)
        {
            services.AddWorkflow();
            services.AddSingleton<IConfigurationService, DefaultConfigurationService>();
            services.AddMediaTasks();

            return services;
        }

        /// <summary>
        /// Uses the media services.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseMediaServices(this IApplicationBuilder app,
            MediaConfiguration configuration)
        {
            app.ApplicationServices.UseMediaServices(configuration);

            return app;
        }

        /// <summary>
        /// Uses the media services.
        /// </summary>
        /// <param name="servicesProvider">The services provider.</param>
        /// <param name="configuratio">The configuratio.</param>
        /// <returns></returns>
        public static IServiceProvider UseMediaServices(this IServiceProvider servicesProvider, MediaConfiguration configuration)
        {
            servicesProvider.GetService<IConfigurationService>().Initialize(configuration);
            return servicesProvider;
        }

        private static void AddMediaTasks(this IServiceCollection services)
        {
            foreach (Type type in typeof(MiddlewareExtensions).Assembly.GetTypes())
            {
                if (type.IsClass 
                    && !type.IsAbstract
                    && typeof(Tasks.MediaTaskBase).IsAssignableFrom(type))
                {
                    services.AddTransient(type);
                }
            }
        }
    }
}
