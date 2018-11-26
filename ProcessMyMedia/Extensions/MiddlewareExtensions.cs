namespace ProcessMyMedia
{
    using System;
    using System.Linq;

    using Microsoft.Extensions.DependencyInjection;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services;
    using ProcessMyMedia.Services.Contract;
    using ProcessMyMedia.Tasks;
    using WorkflowCore.Models;

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
        /// <param name="workflowOptions">The workflow options.</param>
        /// <returns></returns>
        public static IServiceCollection AddMediaServices(this IServiceCollection services,
            AmsConfiguration configuration = null,
            Action<WorkflowOptions> workflowOptions = null)
        {
            services.AddWorkflowServices(workflowOptions);

            services.AddMediaTasks();

            services.AddTransient<IMediaService, AzureMediaServiceV3>();

            if (configuration != null)
            {
                services.AddSingleton<AmsConfiguration>((provider) => configuration);
            }

            return services;
        }

        /// <summary>
        /// Adds the data factory services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="workflowOptions">The workflow options.</param>
        /// <returns></returns>
        public static IServiceCollection AddDataFactoryServices(this IServiceCollection services, 
            AdfConfiguration configuration = null,
            Action<WorkflowOptions> workflowOptions = null)
        {
            services.AddWorkflowServices(workflowOptions);

            services.AddDataTasks();

            services.AddTransient<IDataFactoryService, AzureDataFactoryServiceV2>();

            if (configuration != null)
            {
                services.AddSingleton<AdfConfiguration>((provider) => configuration);
            }

            return services;
        }


        /// <summary>
        /// Adds the media tasks.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddMediaTasks(this IServiceCollection services)
        {
            services.AddTasks<IMediaTask>();
        }

        /// <summary>
        /// Adds the data tasks.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddDataTasks(this IServiceCollection services)
        {
            services.AddTasks<IDataFactoryTask>();
        }

        private static void AddWorkflowServices(this IServiceCollection services, Action<WorkflowOptions> workflowOptions)
        {
            if (services.SingleOrDefault(service => service.ServiceType == typeof(IDelayService)) != null)
            {
                return;
            }

            services.AddWorkflow(workflowOptions);
            services.AddSingleton<IDelayService, DelayService>();
        }

        private static void AddTasks<TTask>(this IServiceCollection services)
            where TTask : ITask
        {
            foreach (Type type in typeof(MiddlewareExtensions).Assembly.GetTypes())
            {
                if (type.IsClass
                    && !type.IsAbstract
                    && typeof(TTask).IsAssignableFrom(type))
                {
                    services.AddTransient(type);
                }
            }
        }
    }
}
