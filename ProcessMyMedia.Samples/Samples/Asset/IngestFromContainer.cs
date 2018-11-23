namespace ProcessMyMedia.Samples
{
    using Microsoft.Extensions.Configuration;

    using WorkflowCore.Interface;


    public class IngestFromContainer : WofkflowSampleBase<IngestFromContainer.IngestFromContainerWorkflow, IngestFromContainer.IngestFromContainerWorkflowData>
    {
        public IngestFromContainer(IConfigurationRoot configuration) : base(configuration)
        {

        }

        protected override IngestFromContainerWorkflowData WorflowDatas => new IngestFromContainerWorkflowData()
        {
            AssetName = "IngestFromContainerAsset",
            ContainerName = "MyContainer"
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
                    //Do somme media processes (encoding...)
                    .Then<Tasks.DeleteAssetTask>()
                    .Input(task => task.AssetName, data => data.AssetName);
            }
        }

        public class IngestFromContainerWorkflowData
        {
            public string AssetName { get; set; }

            public string ContainerName { get; set; }
        }
    }
}
