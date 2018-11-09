# Encoding Samples

## Encoding a file with a BuiltInPreset

The sample encode a file with a BuiltInPreset.

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

The sample encode a file with multiple BuiltInPresets.

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
