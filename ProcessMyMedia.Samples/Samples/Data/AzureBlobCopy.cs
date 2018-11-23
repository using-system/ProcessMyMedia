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
                Type = LinkedServiceType.AzureBlobStorage.ToString(),
                TypeProperties = new
                {
                    connectionString = new
                    {
                        type = "SecureString",
                        value = this.configuration["SamplesConfig:StorageConnectionString"]
                    }
                }
            },
            SourcePath = new AzureBlobDataPath()
            {
                LinkedServiceName = "MyAzureStorage",
                FolderPath = "infolder",
                FileName = "*.*",
                Recursive = false
            },
            DestinationPath = new AzureBlobDataPath()
            {
                LinkedServiceName = "MyAzureStorage",
                FolderPath = "outfolder"
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
                    .Then<Tasks.CopyTask>()
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
