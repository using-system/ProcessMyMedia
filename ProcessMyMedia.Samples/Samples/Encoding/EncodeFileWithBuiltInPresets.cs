namespace ProcessMyMedia.Samples
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Microsoft.Extensions.Configuration;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;

    public class EncodeFileWithBuiltInPresets : WofkflowSampleBase<EncodeFileWithBuiltInPresets.EncodeFileWithBuiltInPresetsWorkflow, EncodeFileWithBuiltInPresets.EncodeFileWithBuiltInPresetsWorkflowData>
    {
        public EncodeFileWithBuiltInPresets(IConfigurationRoot configuration) : base(configuration)
        {

        }

        protected override EncodeFileWithBuiltInPresetsWorkflowData WorflowDatas => new EncodeFileWithBuiltInPresetsWorkflowData()
        {
            Presets =
            {
                Model.BuiltInPreset.H264SingleBitrateSD.ToString(),
                Model.BuiltInPreset.AdaptiveStreaming.ToString()
            }, 
            FilePath = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Asset2\ignite.mp4"),
            DirectoryToDownload = Path.Combine(Directory.GetCurrentDirectory(), "output/", Guid.NewGuid().ToString())
        };

        public class EncodeFileWithBuiltInPresetsWorkflow : IWorkflow<EncodeFileWithBuiltInPresetsWorkflowData>
        {
            public string Id => SampleBase.WORKFLOW_NAME;

            public int Version => 1;

            public void Build(IWorkflowBuilder<EncodeFileWithBuiltInPresetsWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
                    .StartWith<Tasks.EncodeFileBuiltInPresetsTask>()
                        .Input(task => task.FilePath, data => data.FilePath)
                        .Input(task => task.Presets, data => data.Presets)
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

        public class EncodeFileWithBuiltInPresetsWorkflowData
        {
            public EncodeFileWithBuiltInPresetsWorkflowData()
            {
                this.Presets = new List<string>();
            }

            public string FilePath { get; set; }

            public List<string> Presets { get; set; }

            public string DirectoryToDownload { get; set; }

            public List<JobOutputEntity> Outputs { get; set; }
        }
    }
}
