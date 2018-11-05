namespace ProcessMyMedia.Samples
{
    using System.IO;

    using Microsoft.Extensions.Configuration;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    public class AnalyzeMedia : WofkflowSampleBase<AnalyzeMedia.AnalyzeMediaWorkflow, AnalyzeMedia.AnalyzeMediaWorkflowData>
    {
        public AnalyzeMedia(IConfigurationRoot configuration) : base(configuration)
        {

        }

        protected override AnalyzeMediaWorkflowData WorflowDatas => new AnalyzeMediaWorkflowData()
        {
            InputAssetName = "AnalyzeMediaAsset",
            MediaDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Asset1")
        };

        public class AnalyzeMediaWorkflow : IWorkflow<AnalyzeMediaWorkflowData>
        {
            public string Id => SampleBase.WORKFLOW_NAME;

            public int Version => 1;

            public void Build(IWorkflowBuilder<AnalyzeMediaWorkflowData> builder)
            {

                builder
                    .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
                    .StartWith<Tasks.IngestFromDirectoryTask>()
                        .Input(task => task.AssetDirectoryPath, data => data.MediaDirectory)
                        .Input(task => task.AssetName, data => data.InputAssetName)
                    .Then<Tasks.MediaAnalyzerTask>()
                        .Input(task => task.AssetName, data => data.InputAssetName)
                    .Then<Tasks.DeleteAssetTask>()
                        .Input(task => task.AssetName, data => data.InputAssetName);
            }
        }

        public class AnalyzeMediaWorkflowData
        {
            public string MediaDirectory { get; set; }

            public string InputAssetName { get; set; }
        }
    }
}
