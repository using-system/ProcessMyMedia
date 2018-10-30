# Asset Samples

## Ingest from directory

The sample create a new asset and upload files from a directory to the asset, then delete the asset (after doing somes stuffs).

```c#
public class IngestFromDirectoryWorkflow : IWorkflow<IngestFromDirectoryWorkflowData>
{
  public void Build(IWorkflowBuilder<IngestFromDirectoryWorkflowData> builder)
  {
    builder
      .StartWith<Tasks.IngestFromDirectoryTask>()
        .Input(task => task.AssetDirectoryPath, data => data.Directory)
        .Input(task => task.AssetName, data => data.AssetName)
        .Output(data => data.AssetID, task => task.Output.Asset.AssetID)
      //Do somme media processes (encoding...)
      .Then<Tasks.DeleteAssetTask>()
        .Input(task => task.AssetName, data => data.AssetName);
  }
}

public class IngestFromDirectoryWorkflowData
{
  public string Directory { get; set; }

  public string AssetName { get; set; }
  
  public Guid AssetID { get; set; }
}
