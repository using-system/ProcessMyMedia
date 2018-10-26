namespace ProcessMyMedia.Samples
{
    using System;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using ProcessMyMedia.Services;
    using ProcessMyMedia.Services.Contract;
    using WorkflowCore.Interface;

    public class Ingest : SampleBase
    {
        public override void Execute()
        {
            IServiceProvider serviceProvider = ConfigureServices();

            //start the workflow host
            var host = serviceProvider.GetService<IWorkflowHost>();
            host.RegisterWorkflow<IngestWorkflow>();
            host.Start();

            string result = host.StartWorkflow("Ingest").Result;

            Console.ReadLine();


            host.Stop();
        }

        public class IngestWorkflow : IWorkflow
        {
            public string Id => "Ingest";

            public int Version => 1;

            public void Build(IWorkflowBuilder<object> builder)
            {
                builder
                    .StartWith<Tasks.IngestTask>()
                    .Input(task => task.AssetPath, data => @"C:\Users\mnicolescu\Pictures\untitled.png")
                    .Input(task => task.AssetName, data => "MyAsset");
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddWorkflow();
            services.AddSingleton<IConfigurationService, DefaultConfigurationService>();
            //services.AddWorkflow(x => x.UseMongoDB(@"mongodb://localhost:27017", "workflow"));
            services.AddTransient<Tasks.IngestTask>();

            var serviceProvider = services.BuildServiceProvider();

            //config logging
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddDebug();
            return serviceProvider;
        }

    }


}
