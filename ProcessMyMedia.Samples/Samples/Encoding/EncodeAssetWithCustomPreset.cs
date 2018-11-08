namespace ProcessMyMedia.Samples
{
    using System;
    using System.IO;
    using System.Collections.Generic;

    using Microsoft.Extensions.Configuration;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;

    public class EncodeAssetWithCustomPreset : WofkflowSampleBase<EncodeAssetWithCustomPreset.EncodeAssetWithCustomPresetWorkflow, EncodeAssetWithCustomPreset.EncodeAssetWithCustomPresetWorkflowData>
    {
        public EncodeAssetWithCustomPreset(IConfigurationRoot configuration) : base(configuration)
        {

        }


        protected override EncodeAssetWithCustomPresetWorkflowData WorflowDatas => new EncodeAssetWithCustomPresetWorkflowData()
        {
            IntputFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Asset2\ignite.mp4"),
            DirectoryToDownload = Path.Combine(Directory.GetCurrentDirectory(), "output/", Guid.NewGuid().ToString())
        };

        public class EncodeAssetWithCustomPresetWorkflow : IWorkflow<EncodeAssetWithCustomPresetWorkflowData>
        {
            public string Id => SampleBase.WORKFLOW_NAME;

            public int Version => 1;

            public void Build(IWorkflowBuilder<EncodeAssetWithCustomPresetWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
                    .StartWith<Tasks.IngestFileTask>()
                        .Input(task => task.AssetFilePath, data => data.IntputFilePath)
                        .Input(task => task.AssetName, data => data.InputAssetName)
                    .Then<Tasks.EncodeAssetTask>()
                        .Input(task => task.Input, data => new JobInputEntity() { Name = data.InputAssetName })
                        .Input(task => task.EncodingOutput, data => data.EncodingOutput)
                        .Output(data => data.Outputs, task => task.Output.Job.Outputs)
                    .ForEach(data => data.Outputs)
                        .Do(iteration => iteration
                            .StartWith<Tasks.DownloadAssetTask>()
                                .Input(task => task.AssetName, (data, context) => ((JobOutputEntity)context.Item).Name)
                                .Input(task => task.DirectoryToDownload, (data, context) => Path.Combine(data.DirectoryToDownload, ((JobOutputEntity)context.Item).Label))
                            .Then<Tasks.DeleteAssetTask>()
                                .Input(task => task.AssetName, (data, context) => ((JobOutputEntity)context.Item).Name));
            }
        }

        public class EncodeAssetWithCustomPresetWorkflowData
        {
            public string InputAssetName { get; set; }

            public string IntputFilePath { get; set; }

            public CustomPresetEncodingOutput EncodingOutput { get; set; }

            public string DirectoryToDownload { get; set; }

            public List<JobOutputEntity> Outputs { get; set; }
        }
    }
}
