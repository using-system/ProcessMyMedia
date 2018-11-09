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
                        FilenamePattern = "{Basename}_{Bitrate}.{Extension}",
                        KeyFrameInterval = "00:00:02",
                        SceneChangeDetection = true,
                        Layers =
                        {
                            //new H264VideoLayer()
                            //{
                            //    Bitrate = 4500,
                            //    Height = "1280",
                            //    Width = "720"
                            //},
                            new H264VideoLayer()
                            {
                                Bitrate = 2200,
                                MaxBitrate = 2200,
                                Height = "480",
                                Width = "848",
                                BufferWindow = "00:00:05",
                                AdaptiveBFrame = true,
                                FrameRate = "0/1",
                                ReferenceFrames = 3,
                                Level = "auto",
                                Slices = 0,
                                BFrames = 3,
                                Profile = "Auto",
                                EntropyMode = "Cabac"
                            }
                        }
                    }
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
                            .Input(task => task.Input, data => new JobInputEntity() { Name = data.InputAssetName })
                            .Input(task => task.EncodingOutput, data => data.EncodingOutput)
                            .Output(data => data.Outputs, task => task.Output.Job.Outputs)
                        .ForEach(data => data.Outputs)
                            .Do(iteration => iteration
                                .StartWith<Tasks.DownloadAssetTask>()
                                    .Input(task => task.AssetName, (data, context) => ((JobOutputEntity)context.Item).Name)
                                    .Input(task => task.DirectoryToDownload, (data, context) => Path.Combine(data.DirectoryToDownload, ((JobOutputEntity)context.Item).Label))
                                .Then<Tasks.DeleteAssetTask>()
                                    .Input(task => task.AssetName, (data, context) => ((JobOutputEntity)context.Item).Name))
                        .Then<Tasks.DeleteAssetTask>()
                            .Input(task => task.AssetName, (data) => data.InputAssetName))
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

            public List<JobOutputEntity> Outputs { get; set; }
        }
    }
}
/*
 * 
 * 
 *
 <Preset xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Version="1.0" xmlns="http://www.windowsazure.com/media/encoding/Preset/2014/03">  
  <Encoding>  
    <H264Video>  
      <KeyFrameInterval>00:00:02</KeyFrameInterval>  
      <SceneChangeDetection>true</SceneChangeDetection>  
      <H264Layers>  
        <H264Layer>  
          <Bitrate>4500</Bitrate>  
          <Width>1280</Width>  
          <Height>720</Height>  
          <FrameRate>0/1</FrameRate>  
          <Profile>Auto</Profile>  
          <Level>auto</Level>  
          <BFrames>3</BFrames>  
          <ReferenceFrames>3</ReferenceFrames>  
          <Slices>0</Slices>  
          <AdaptiveBFrame>true</AdaptiveBFrame>  
          <EntropyMode>Cabac</EntropyMode>  
          <BufferWindow>00:00:05</BufferWindow>  
          <MaxBitrate>4500</MaxBitrate>  
        </H264Layer>  
      </H264Layers>  
      <Chapters />  
    </H264Video>  
    <AACAudio>  
      <Profile>AACLC</Profile>  
      <Channels>2</Channels>  
      <SamplingRate>48000</SamplingRate>  
      <Bitrate>128</Bitrate>  
    </AACAudio>  
  </Encoding>  
  <Outputs>  
    <Output FileName="{Basename}_{Width}x{Height}_{VideoBitrate}.mp4">  
      <MP4Format />  
    </Output>  
  </Outputs>  
</Preset>  

<Preset xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Version="1.0" xmlns="http://www.windowsazure.com/media/encoding/Preset/2014/03">  
  <Encoding>  
    <H264Video>  
      <KeyFrameInterval>00:00:02</KeyFrameInterval>  
      <SceneChangeDetection>true</SceneChangeDetection>  
      <H264Layers>  
        <H264Layer>  
          <Bitrate>2200</Bitrate>  
          <Width>848</Width>  
          <Height>480</Height>  
          <FrameRate>0/1</FrameRate>  
          <Profile>Auto</Profile>  
          <Level>auto</Level>  
          <BFrames>3</BFrames>  
          <ReferenceFrames>3</ReferenceFrames>  
          <Slices>0</Slices>  
          <AdaptiveBFrame>true</AdaptiveBFrame>  
          <EntropyMode>Cabac</EntropyMode>  
          <BufferWindow>00:00:05</BufferWindow>  
          <MaxBitrate>2200</MaxBitrate>  
        </H264Layer>  
      </H264Layers>  
      <Chapters />  
    </H264Video>  
    <AACAudio>  
      <Profile>AACLC</Profile>  
      <Channels>2</Channels>  
      <SamplingRate>48000</SamplingRate>  
      <Bitrate>128</Bitrate>  
    </AACAudio>  
  </Encoding>  
  <Outputs>  
    <Output FileName="{Basename}_{Width}x{Height}_{VideoBitrate}.mp4">  
      <MP4Format />  
    </Output>  
  </Outputs>  
</Preset> 
 */
