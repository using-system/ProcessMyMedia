namespace ProcessMyMedia.Samples
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Configuration;

    using WorkflowCore.Interface;


    public class IngestFromContainer : WofkflowSampleBase<IngestFromContainer.IngestFromContainerWorkflow, IngestFromContainer.IngestFromContainerWorkflowData>
    {
        public IngestFromContainer(IConfigurationRoot configuration) : base(configuration)
        {

        }

        protected override IngestFromContainerWorkflowData WorflowDatas => new IngestFromContainerWorkflowData()
        {
            AssetName = "4e1fe20b-a06c-4431-8a74-7f570a151bb7",
            ContainerName = "4e1fe20b-a06c-4431-8a74-7f570a151bb7",
            DirectoryToDownload = Path.Combine(Directory.GetCurrentDirectory(), "output/", Guid.NewGuid().ToString()),
        };

        public class IngestFromContainerWorkflow : IWorkflow<IngestFromContainerWorkflowData>
        {
            public string Id => SampleBase.WORKFLOW_NAME;

            public int Version => 1;

            public void Build(IWorkflowBuilder<IngestFromContainerWorkflowData> builder)
            {
                builder
                    .StartWith<Tasks.IngestFromContainerTask>()
                        .Input(task => task.AssetName, data => data.AssetName)
                        .Input(task => task.ContainerName, data => data.AssetName)
                    .Then<Tasks.DownloadAssetTask>()
                        .Input(task => task.AssetName, data => data.AssetName)
                        .Input(task => task.DirectoryToDownload, data => data.DirectoryToDownload);
            }
        }

        public class IngestFromContainerWorkflowData
        {
            public string AssetName { get; set; }

            public string ContainerName { get; set; }

            public string DirectoryToDownload { get; set; }
        }
    }
}
