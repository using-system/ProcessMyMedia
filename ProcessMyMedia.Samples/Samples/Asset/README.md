
```c#
builder
  .StartWith<Tasks.IngestFromDirectoryTask>()
    .Input(task => task.AssetDirectoryPath, data => data.Directory)
    .Input(task => task.AssetName, data => data.AssetName)
    .Output(data => data.AssetID, task => task.Output.Asset.AssetID)
  //Do somme media processes (encoding...)
  .Then<Tasks.DeleteAssetTask>()
    .Input(task => task.AssetName, data => data.AssetName);
```
