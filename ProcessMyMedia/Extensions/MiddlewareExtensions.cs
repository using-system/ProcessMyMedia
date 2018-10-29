namespace ProcessMyMedia
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    using ProcessMyMedia.Model;

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
        public static IServiceCollection AddMediaServices(this IServiceCollection services, MediaConfiguration configuration = null)
        {
            services.AddWorkflow();
            services.AddMediaTasks();

            if (configuration != null)
            {
                services.AddSingleton<MediaConfiguration>((provider) => configuration);
            }

            return services;
        }


        private static void AddMediaTasks(this IServiceCollection services)
        {
            foreach (Type type in typeof(MiddlewareExtensions).Assembly.GetTypes())
            {
                if (type.IsClass 
                    && !type.IsAbstract
                    && typeof(Tasks.IMediaTask).IsAssignableFrom(type))
                {
                    services.AddTransient(type);
                }
            }
        }
    }
}
