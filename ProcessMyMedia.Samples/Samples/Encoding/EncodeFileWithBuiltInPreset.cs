﻿namespace ProcessMyMedia.Samples
{
    using System;
    using System.IO;
    using System.Linq;

    using Microsoft.Extensions.Configuration;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    public class EncodeFileWithBuiltInPreset : WofkflowSampleBase<EncodeFileWithBuiltInPreset.EncodeFileWithBuiltInPresetWorkflow, EncodeFileWithBuiltInPreset.EncodeFileWithBuiltInPresetWorkflowData>
    {
        public EncodeFileWithBuiltInPreset(IConfigurationRoot configuration) : base(configuration)
        {

        }

        protected override EncodeFileWithBuiltInPresetWorkflowData WorflowDatas => new EncodeFileWithBuiltInPresetWorkflowData()
        {
            Preset = Model.BuiltInPreset.H264SingleBitrateSD.ToString(),
            FilePath = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Asset2\ignite.mp4"),
            DirectoryToDownload = Path.Combine(Directory.GetCurrentDirectory(), "output/", Guid.NewGuid().ToString())
        };

        public class EncodeFileWithBuiltInPresetWorkflow : IWorkflow<EncodeFileWithBuiltInPresetWorkflowData>
        {
            public string Id => SampleBase.WORKFLOW_NAME;

            public int Version => 1;

            public void Build(IWorkflowBuilder<EncodeFileWithBuiltInPresetWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
                        .StartWith<Tasks.EncodeFileBuiltInPresetTask>()
                        .Input(task => task.FilePath, data => data.FilePath)
                        .Input(task => task.Preset, data => data.Preset)
                        .Output(data => data.OutputAssetName, task => task.Output.Job.Outputs.First().Name)
                    .Then<Tasks.DownloadAssetTask>()
                        .Input(task => task.AssetName, data => data.OutputAssetName)
                        .Input(task => task.DirectoryToDownload, data => data.DirectoryToDownload)
                    .Then<Tasks.DeleteAssetTask>()
                        .Input(task => task.AssetName, data => data.OutputAssetName);
            }
        }

        public class EncodeFileWithBuiltInPresetWorkflowData
        {
            public string FilePath { get; set; }

            public string Preset { get; set; }

            public string DirectoryToDownload { get; set; } 

            public string OutputAssetName { get; set; }
        }
    }
}
