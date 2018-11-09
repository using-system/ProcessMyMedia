# Encoding Samples

## Encoding a file with a BuiltInPreset

The sample create a new asset and upload files from a directory to the asset, then delete the asset (after doing somes stuffs).

```c#
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
