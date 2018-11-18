namespace ProcessMyMedia.Samples
{
    using Microsoft.Extensions.Configuration;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;

    public class FtpCopy : WofkflowSampleBase<FtpCopy.FtpCopyWorkflow, FtpCopy.FtpCopyWorkflowData>
    {
        public FtpCopy(IConfigurationRoot configuration) : base(configuration)
        {

        }

        protected override FtpCopyWorkflowData WorflowDatas => new FtpCopyWorkflowData()
        {
            FtpServer = new LinkedServiceEntity()
            {
                Name = "MyFtpServer",
                Type = "FtpServer",
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
            SourcePath = new FtpDataPath()
            {
                Type = DataPathType.Ftp,
                LinkedServiceName = "MyFtpServer",
                FolderPath = "in",
                FileName = "*.mpg",
                Recursive = false,
                PathProperties = new
                {
                    folderPath = "in",
                    fileName = "*.mpg"
                },
                CopyProperties = new
                {
                    recursive = false
                }
            },
            DestinationPath = new FtpDataPath()
            {
                Type = DataPathType.Ftp,
                LinkedServiceName = "MyFtpServer",
                FolderPath = "out",
                Recursive = false,
                PathProperties = new
                {
                    folderPath = "out"
                },
                CopyProperties = new
                {
                    recursive = false
                }
            }
        };

        public class FtpCopyWorkflow : IWorkflow<FtpCopyWorkflowData>
        {
            public string Id => SampleBase.WORKFLOW_NAME;

            public int Version => 1;


            public void Build(IWorkflowBuilder<FtpCopyWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
                    .StartWith<Tasks.CreateLinkedServiceTask>()
                        .Input(task => task.LinkedServiceToCreate, data => data.FtpServer)
                    .Then<Tasks.GenericCopyTask>()
                        .Input(task => task.SourcePath, data => data.SourcePath)
                        .Input(task => task.DestinationPath, data => data.DestinationPath);
            }
        }

        public class FtpCopyWorkflowData
        {
            public LinkedServiceEntity FtpServer { get; set; }

            public DataPath SourcePath { get; set; }

            public DataPath DestinationPath { get; set; }
        }
    }
}
