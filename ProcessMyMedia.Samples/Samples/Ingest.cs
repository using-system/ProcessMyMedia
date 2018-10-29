using ProcessMyMedia.Model;

namespace ProcessMyMedia.Samples
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using WorkflowCore.Interface;

    public class Ingest : SampleBase
    {
        public Ingest(IConfigurationRoot configuration) : base(configuration)
        {

        }

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
                    .StartWith<Tasks.IngestFromDirectoryTask>()
                    .Input(task => task.AssetDirectoryPath, data => @"C:\Users\mnicolescu\Documents\Asset")
                    .Input(task => task.AssetName, data => "MyAsset");
            }
        }

    }


}
