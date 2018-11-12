namespace ProcessMyMedia.Samples
{
    using System;
    using System.Linq;
    using System.IO;

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
            InputAssetName = Guid.NewGuid().ToString(),
            IntputFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Asset2\ignite.mp4"),
            DirectoryToDownload = Path.Combine(Directory.GetCurrentDirectory(), "output/", Guid.NewGuid().ToString()),
            EncodingOutput = new CustomPresetEncodingOutput()
            {
                PresetName = "EncodeAssetWithCustomPreset",
                Codecs =
                {
                    new H264VideoCodec()
                    {
                        FilenamePattern = "Video-{Basename}-{Label}-{Bitrate}{Extension}",
                        KeyFrameInterval = "00:00:02",
                        SceneChangeDetection = false,
                        Layers =
                        {
                            new H264VideoLayer()
                            {
                                Label = "SD",
                                Bitrate = 600000,
                                Height = "640",
                                Width = "360"
                            },
                            new H264VideoLayer()
                            {
                                Label = "HD",
                                Bitrate = 1000000,
                                Height = "1280",
                                Width = "720"
                            },
                        }
                    },
                    new AacAudioCodec()
                    {
                        Bitrate = 128000,
                        Channels = 2,
                        Profile = "AACLC",
                        SamplingRate = 48000
                    }
                },
                ThumbnailsOptions = new ThumbnailsOptions()
                {
                    FilenamePattern = "Thumbnail-{Basename}-{Index}{Extension}",
                    GeneratePng = true,
                    GenerateJpg = true,
                    Height = "50%",
                    Width = "50%",
                    Start = "10%",
                    Step = "10%",
                    Range = "90%"
                }
            }
        };

        public class EncodeAssetWithCustomPresetWorkflow : IWorkflow<EncodeAssetWithCustomPresetWorkflowData>
        {
            public string Id => SampleBase.WORKFLOW_NAME;

            public int Version => 1;

            public void Build(IWorkflowBuilder<EncodeAssetWithCustomPresetWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
                    .StartWith(context => ExecutionResult.Next())
                    .Saga(saga => saga
                        .StartWith<Tasks.IngestFileTask>()
                            .Input(task => task.AssetFilePath, data => data.IntputFilePath)
                            .Input(task => task.AssetName, data => data.InputAssetName)
                        .Then<Tasks.EncodeAssetTask>()
                            .Input(task => task.Priority, data => JobPriority.High)
                            .Input(task => task.Input, data => new JobInputEntity() { Name = data.InputAssetName })
                            .Input(task => task.EncodingOutput, data => data.EncodingOutput)
                            .Output(data => data.OutputAssetName, task => task.Output.Job.Outputs.First().Name)
                        .Then<Tasks.DownloadAssetTask>()
                            .Input(task => task.AssetName, data => data.OutputAssetName)
                            .Input(task => task.DirectoryToDownload, data => data.DirectoryToDownload)
                        .Then<Tasks.DeleteAssetTask>()
                            .Input(task => task.AssetName, data => data.OutputAssetName))
                    .CompensateWith<Tasks.DeleteAssetTask>(compensate => compensate
                        .Input(task => task.AssetName, data => data.InputAssetName));
            }
        }

        public class EncodeAssetWithCustomPresetWorkflowData
        {
            public string InputAssetName { get; set; }

            public string IntputFilePath { get; set; }

            public CustomPresetEncodingOutput EncodingOutput { get; set; }

            public string DirectoryToDownload { get; set; }

            public string OutputAssetName { get; set; }
        }
    }
}