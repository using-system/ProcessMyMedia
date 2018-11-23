namespace ProcessMyMedia.Samples
{
    using Microsoft.Extensions.Configuration;

    using WorkflowCore.Interface;

    using ProcessMyMedia.Model;

    public class CopyWithGenericPath : WofkflowSampleBase<CopyWithGenericPath.CopyWithGenericPathWorkflow, CopyWithGenericPath.CopyWithGenericPathWorkflowData>
    {

        public CopyWithGenericPath(IConfigurationRoot configuration) : base(configuration)
        {

        }

        protected override CopyWithGenericPathWorkflowData WorflowDatas => new CopyWithGenericPathWorkflowData()
        {
            FtpServer = new LinkedServiceEntity()
            {
                Name = "MyFtpServer",
                Type = LinkedServiceType.FtpServer.ToString(),
                TypeProperties = new
                {
                    host = "localhost",
                    port = 21,
                    enableSsl = false,
                    authenticationType = "Basic",
                    username = "user",
                    password = new
                    {
                        type = "SecureString",
                        value = "password"
                    }
                }
            },
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
            SourcePath = new GenericDataPath()
            {
                LinkedServiceName = "MyFtpServer",
                Type = LinkedServiceType.FtpServer,
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
                Type = LinkedServiceType.AzureBlobStorage,
                PathProperties = new
                {
                    folderPath = "outfolder"
                }
            }
        };

        public class CopyWithGenericPathWorkflow : IWorkflow<CopyWithGenericPathWorkflowData>
        {
            public string Id => SampleBase.WORKFLOW_NAME;

            public int Version => 1;


            public void Build(IWorkflowBuilder<CopyWithGenericPathWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowCore.Models.WorkflowErrorHandling.Terminate)
                    .StartWith<Tasks.CreateLinkedServiceTask>()
                        .Input(task => task.LinkedServiceToCreate, data => data.FtpServer)
                    .Then<Tasks.CopyTask>()
                        .Input(task => task.SourcePath, data => data.SourcePath)
                        .Input(task => task.DestinationPath, data => data.DestinationPath);
            }
        }

        public class CopyWithGenericPathWorkflowData
        {
            public LinkedServiceEntity FtpServer { get; set; }

            public LinkedServiceEntity AzureStorageResource { get; set; }

            public DataPath SourcePath { get; set; }

            public DataPath DestinationPath { get; set; }
        }
    }
}
