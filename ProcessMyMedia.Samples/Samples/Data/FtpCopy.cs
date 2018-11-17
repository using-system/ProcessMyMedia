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
            SourcePath = new DatasetEntity()
            {
                Type = "FileShare",
                LinkedServiceName = "MyFtpServer",
                TypeProperties = new
                {
                    folderPath = "in",
                    fileName = "*.mpg"
                }
            },
            DestinationPath = new DatasetEntity()
            {
                Type = "FileShare",
                LinkedServiceName = "MyFtpServer",
                TypeProperties = new
                {
                    folderPath = "out"
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
                        .Input(task => task.DatasetInput, data => data.SourcePath)
                        .Input(task => task.DatasetOutput, data => data.DestinationPath);
            }
        }

        public class FtpCopyWorkflowData
        {
            public LinkedServiceEntity FtpServer { get; set; }

            public DatasetEntity SourcePath { get; set; }

            public DatasetEntity DestinationPath { get; set; }
        }
    }
}
