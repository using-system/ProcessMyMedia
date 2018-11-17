namespace ProcessMyMedia.Samples
{
    using Microsoft.Extensions.Configuration;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;

    public class AzureBlobCopy : WofkflowSampleBase<AzureBlobCopy.AzureBlobCopyWorkflow, AzureBlobCopy.AzureBlobCopyWorkflowData>
    {
        public AzureBlobCopy(IConfigurationRoot configuration) : base(configuration)
        {

        }

        protected override AzureBlobCopyWorkflowData WorflowDatas => new AzureBlobCopyWorkflowData()
        {
            AzureStorageResource = new LinkedServiceEntity()
            {
                Name = "MyAzureStorage",
                Type = "AzureBlobStorage",
                TypeProperties = new
                {
                    serviceEndpoint = "https://mnuadf.blob.core.windows.net/" //the azure blob storage must be associated with the data factory
                }
            },
            SourcePath = new DataPath()
            {
                Type = DataPathType.AzureBlobStorage,
                LinkedServiceName = "MyAzureStorage",
                PathProperties = new
                {
                    folderPath = "infolder",
                    fileName = "*.*"
                },
                CopyProperties = new
                {
                    recursive = false
                }
            },
            DestinationPath = new DataPath()
            {
                Type = DataPathType.AzureBlobStorage,
                LinkedServiceName = "MyAzureStorage",
                PathProperties = new
                {
                    folderPath = "outfolder"
                },
                CopyProperties = new
                {
                    recursive = false
                }
            }
        };

        public class AzureBlobCopyWorkflow : IWorkflow<AzureBlobCopyWorkflowData>
        {
            public string Id => SampleBase.WORKFLOW_NAME;

            public int Version => 1;


            public void Build(IWorkflowBuilder<AzureBlobCopyWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
                    .StartWith<Tasks.CreateLinkedServiceTask>()
                        .Input(task => task.LinkedServiceToCreate, data => data.AzureStorageResource)
                    .Then<Tasks.GenericCopyTask>()
                        .Input(task => task.SourcePath, data => data.SourcePath)
                        .Input(task => task.DestinationPath, data => data.DestinationPath);
            }
        }

        public class AzureBlobCopyWorkflowData
        {
            public LinkedServiceEntity AzureStorageResource { get; set; }

            public DataPath SourcePath { get; set; }

            public DataPath DestinationPath { get; set; }
        }
    }
}
