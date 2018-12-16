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
            SourcePath = new FtpDataPath()
            {
                LinkedServiceName = "MyFtpServer",
                FolderPath = "in",
                FileName = "*.mpg",
                Recursive = false,
            },
            DestinationPath = new FtpDataPath()
            {
                LinkedServiceName = "MyFtpServer",
                FolderPath = "out"
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
                         .Input(task => task.Name, data => data.FtpServer.Name)
                         .Input(task => task.Type, data => data.FtpServer.Type)
                         .Input(task => task.Properties, data => data.FtpServer.TypeProperties)
                    .Then<Tasks.CopyTask>()
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
