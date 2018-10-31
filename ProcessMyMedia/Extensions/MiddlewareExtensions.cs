namespace ProcessMyMedia
{
    using System;

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
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddMediaServices(this IServiceCollection services, WamsConfiguration configuration = null)
        {
            services.AddWorkflow();
            services.AddMediaTasks();

            services.AddTransient<IMediaService, AzureMediaServiceV3>();

            if (configuration != null)
            {
                services.AddSingleton<WamsConfiguration>((provider) => configuration);
            }

            return services;
        }


        /// <summary>
        /// Adds the media tasks.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddMediaTasks(this IServiceCollection services)
        {
            foreach (Type type in typeof(MiddlewareExtensions).Assembly.GetTypes())
            {
                if (type.IsClass 
                    && !type.IsAbstract
                    && typeof(Tasks.ITask).IsAssignableFrom(type))
                {
                    services.AddTransient(type);
                }
            }
        }
    }
}
