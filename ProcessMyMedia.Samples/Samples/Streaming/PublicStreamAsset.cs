namespace ProcessMyMedia.Samples
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.Extensions.Configuration;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    public class PublicStreamAsset : WofkflowSampleBase<PublicStreamAsset.PublicStreamAssetWorkflow, PublicStreamAsset.PublicStreamAssetWorkflowData>
    {
        public PublicStreamAsset(IConfigurationRoot configuration) : base(configuration)
        {

        }

        protected override PublicStreamAssetWorkflowData WorflowDatas => new PublicStreamAssetWorkflowData()
        {
            IntputFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Asset2\ignite.mp4"),
        };

        public class PublicStreamAssetWorkflow : IWorkflow<PublicStreamAssetWorkflowData>
        {
            public string Id => SampleBase.WORKFLOW_NAME;

            public int Version => 1;

            public void Build(IWorkflowBuilder<PublicStreamAssetWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
                    .StartWith<Tasks.EncodeFileBuiltInPresetTask>()
                        .Input(task => task.FilePath, data => data.IntputFilePath)
                        .Input(task => task.Preset, data => Model.BuiltInPreset.AdaptiveStreaming.ToString())
                        .Output(data => data.AssetName, task => task.Output.Job.Outputs.First().Name)
                    .Then<Tasks.StreamTask>()
                        .Input(task => task.AssetName, data => data.AssetName)
                        .Output(data => data.StreamingUrls, task => task.Output.StreamingUrls);

            }
        }

        public class PublicStreamAssetWorkflowData
        {
            public string IntputFilePath { get; set; }

            public string AssetName { get; set; }

            public List<string> StreamingUrls { get; set; }
        }
    }
}
