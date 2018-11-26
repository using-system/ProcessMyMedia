# Analyzing Samples

## Analyse Asset

The sample : 

1. Ingest a new asset
2. Launch the analyse task
2. Download the result then remove input/output asset 


```c#

 public class AnalyzeAssetWorkflow : IWorkflow<AnalyzeAssetWorkflowData>
{
	public void Build(IWorkflowBuilder<AnalyzeAssetWorkflowData> builder)
	{
		builder
		.UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
		.StartWith<Tasks.IngestFromDirectoryTask>()
			.Input(task => task.AssetDirectoryPath, data => data.MediaDirectory)
			.Input(task => task.AssetName, data => data.InputAssetName)
		.Then<Tasks.AnalyzeAssetTask>()
			.Input(task => task.AssetName, data => data.InputAssetName)
			.Output(data => data.OutputAssetName, task => task.Output.Result.OutputAssetName)
		.Then<Tasks.DeleteAssetTask>()
			.Input(task => task.AssetName, data => data.InputAssetName)
		.If(data => !string.IsNullOrEmpty(data.OutputAssetName))
		.Do(then =>
			//Get the analysing result
			then.StartWith<Tasks.DownloadAssetTask>()
				.Input(task => task.AssetName, data => data.OutputAssetName)
				.Input(task => task.DirectoryToDownload, data => data.DirectoryToDownload)
			//Delete output asset (analysing result)
			.Then< Tasks.DeleteAssetTask>()
				.Input(task => task.AssetName, data => data.OutputAssetName));
	}
}

public class AnalyzeAssetWorkflowData
{
	public string MediaDirectory { get; set; }

	public string InputAssetName { get; set; }

	public string OutputAssetName { get; set; }

	public string DirectoryToDownload { get; set; }
}

```