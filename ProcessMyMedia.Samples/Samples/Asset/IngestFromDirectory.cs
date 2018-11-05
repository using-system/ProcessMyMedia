namespace ProcessMyMedia.Samples
{
    using System.IO;


    using Microsoft.Extensions.Configuration;

    using WorkflowCore.Interface;

    public class IngestFromDirectory : WofkflowSampleBase<IngestFromDirectory.IngestFromDirectoryWorkflow, IngestFromDirectory.IngestFromDirectoryWorkflowData>
    {
        public IngestFromDirectory(IConfigurationRoot configuration) : base(configuration)
        {

        }

        protected override IngestFromDirectoryWorkflowData WorflowDatas => new IngestFromDirectoryWorkflowData()
        {
            AssetName = "Asset1",
            Directory = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Asset1")
        };

        public class IngestFromDirectoryWorkflow : IWorkflow<IngestFromDirectoryWorkflowData>
        {
            public string Id => SampleBase.WORKFLOW_NAME;

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

            public string AssetID { get; set; }
        }

    }


}
