namespace PMM.Samples
{
    using System;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

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

            host.StartWorkflow("Ingest");

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
                    .StartWith<Tasks.IngestTask>();
            }
        }

        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
            services.AddWorkflow();
            //services.AddWorkflow(x => x.UseMongoDB(@"mongodb://localhost:27017", "workflow"));

            var serviceProvider = services.BuildServiceProvider();

            //config logging
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddDebug();
            return serviceProvider;
        }

    }


}
