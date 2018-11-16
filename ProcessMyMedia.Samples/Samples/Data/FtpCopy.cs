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
            SourceServer = new LinkedServiceEntity()
            {
                Name = "LocalFtp",
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
            SourcePath = new DatasetEntity()
            {
                Name = "LocalFtpPath",
                Type = "FileShare",
                LinkedServiceName = "LocalFtp",
                TypeProperties = new
                {
                    folderPath = "myfolder/subfolder",
                    fileName = "test.mpg"
                }
            },
            DestinationServer = new LinkedServiceEntity()
            {
                Name = "LocalShare",
                Type = "FileServer",
                TypeProperties = new
                {
                    host = "\\localhost\\share"
                }
            },
            DestinationPath = new DatasetEntity()
            {
                Name = "LocalSharePath",
                Type = "FileShare",
                LinkedServiceName = "LocalShare",
                TypeProperties = new
                {
                    folderPath = "folder1"
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
                        .Input(task => task.LinkedServiceToCreate, data => data.SourceServer)
                    .Then<Tasks.CreateLinkedServiceTask>()
                        .Input(task => task.LinkedServiceToCreate, data => data.DestinationServer)
                    .Then<Tasks.GenericCopyTask>()
                        .Input(task => task.DatasetInput, data => data.SourcePath)
                        .Input(task => task.DatasetOutput, data => data.DestinationPath);
            }
        }

        public class FtpCopyWorkflowData
        {
            public LinkedServiceEntity SourceServer { get; set; }

            public LinkedServiceEntity DestinationServer { get; set; }

            public DatasetEntity SourcePath { get; set; }

            public DatasetEntity DestinationPath { get; set; }
        }
    }
}
