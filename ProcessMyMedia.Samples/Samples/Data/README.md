# Data Factory Samples

## Copy from FTP to AzureStorage


```c#
 public class CopyWithGenericPathWorkflow : IWorkflow<CopyWithGenericPathWorkflowData>
{
	public void Build(IWorkflowBuilder<CopyWithGenericPathWorkflowData> builder)
	{
		builder
			.UseDefaultErrorBehavior(WorkflowCore.Models.WorkflowErrorHandling.Terminate)
			.StartWith<Tasks.CopyTask>()
				.Input(task => task.SourcePath, data => data.SourcePath)
				.Input(task => task.DestinationPath, data => data.DestinationPath);
				
	}
}

public class CopyWithGenericPathWorkflowData
{
	public DataPath SourcePath { get; set; }

	public DataPath DestinationPath { get; set; }
}

var datas = new CopyWithGenericPathWorkflowData()
{
	SourcePath = new GenericDataPath()
	{
		LinkedServiceName = "MyFtpServer",
		Type = DataPathType.Ftp,
		PathProperties = new
		{
			folderPath = "in",
			fileName = "*.mpg "
		},
		ActivityProperties = new
		{
			recursive = false
		}
	},
	DestinationPath = new GenericDataPath()
	{
		LinkedServiceName = "MyAzureStorage",
		Type = DataPathType.AzureBlobStorage,
		PathProperties = new
		{
			folderPath = "outfolder"
		}
	}
};
```
