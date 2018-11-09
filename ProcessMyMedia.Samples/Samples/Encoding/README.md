# Encoding Samples

## Encoding a file with a BuiltInPreset

The sample : 

1. Encode a file with a BuiltInPreset
2. Download the output asset
2. Delete the output asset

```c#
public class EncodeFileWithBuiltInPresetWorkflow : IWorkflow<EncodeFileWithBuiltInPresetWorkflowData>
{
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
```
## Encoding a file with multiple BuiltInPresets

The sample : 

1. Encode a file with multiple BuiltInPresets.
2. Download the output assets
2. Delete the output assets

```c#
public class EncodeFileWithBuiltInPresetsWorkflow : IWorkflow<EncodeFileWithBuiltInPresetsWorkflowData>
{
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
        .Input(task => task.DirectoryToDownload, (data, context) => 
            Path.Combine(data.DirectoryToDownload, ((JobOutputEntity)context.Item).Label))
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
```
## Encoding a asset with a custom preset

The sample : 

1. Ingest a new asset with the task IngestFileTask.
1. Encode an asset with a custom Azure Media Encoder Preset.
2. Download the output asset
2. Delete the output and input asset

```c#
public class EncodeAssetWithCustomPresetWorkflow : IWorkflow<EncodeAssetWithCustomPresetWorkflowData>
{

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

var customPreset = new CustomPresetEncodingOutput()
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
    }
}
```
