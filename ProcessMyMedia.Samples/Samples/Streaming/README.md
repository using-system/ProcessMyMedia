# Streaming Samples

## Streaming an asset

The sample : 

1. Encode a file with the BuiltInPreset AdaptiveStreaming required for streaming scenarios
2. Stream the asset

```c#
public class PublicStreamAssetWorkflow : IWorkflow<PublicStreamAssetWorkflowData>
{

	public void Build(IWorkflowBuilder<PublicStreamAssetWorkflowData> builder)
	{
		builder
		.UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
		.StartWith<Tasks.EncodeFileBuiltInPresetTask>()
			.Input(task => task.FilePath, data => data.IntputFilePath)
			.Input(task => task.Preset, data => Model.BuiltInPreset.AdaptiveStreaming.ToString())
			.Output(data => data.AssetName, task => task.Output.Job.Outputs.First().Name)
		.Then<Tasks.StreamAssetTask>()
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
```