namespace ProcessMyMedia.Samples
{
    using System;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using WorkflowCore.Interface;

    public class IngestFromDirectory : SampleBase
    {
        public IngestFromDirectory(IConfigurationRoot configuration) : base(configuration)
        {

        }

        public override void Execute()
        {
            IServiceProvider serviceProvider = ConfigureServices();

            //start the workflow host
            var host = serviceProvider.GetService<IWorkflowHost>();
            host.RegisterWorkflow<IngestFromDirectoryWorkflow, IngestFromDirectoryWorkflowData>();
            host.Start();

            string result = host.StartWorkflow<IngestFromDirectoryWorkflowData>("Ingest", data: new IngestFromDirectoryWorkflowData()
            {
                AssetName = "MyAsset",
                Directory = @"C:\Users\mnicolescu\Documents\Asset"
            }).Result;

            Console.WriteLine(("Press Enter to stop the workflow host"));
            Console.ReadLine();

            host.Stop();
        }

        public class IngestFromDirectoryWorkflow : IWorkflow<IngestFromDirectoryWorkflowData>
        {
            public string Id => "IngestFromDirectoryWorkflow";

            public int Version => 1;

            public void Build(IWorkflowBuilder<IngestFromDirectoryWorkflowData> builder)
            {
                builder
                    .StartWith<Tasks.IngestFromDirectoryTask>()
                        .Input(task => task.AssetDirectoryPath, data => data.Directory)
                        .Input(task => task.AssetName, data => data.AssetName)
                        .Output(data => data.AssetID, task => task.Output.Asset.AssetID)
                    //Do somme media processes (encoding...)
                    .Then<Tasks.DeleteAssetTask>()
                        .Input(task => task.AssetName, data => data.AssetName);
            }
        }

        public class IngestFromDirectoryWorkflowData
        {
            public string Directory { get; set; }

            public string AssetName { get; set; }

            public Guid AssetID { get; set; }
        }

    }


}
