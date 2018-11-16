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
            Source = new LinkedServiceEntity()
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
            Destination = new LinkedServiceEntity()
            {
                Name = "LocalShare",
                Type = "FileServer",
                TypeProperties = new
                {
                    host = "\\localhost"
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
                        .Input(task => task.LinkedServiceToCreate, data => data.Source)
                    .Then<Tasks.CreateLinkedServiceTask>()
                        .Input(task => task.LinkedServiceToCreate, data => data.Destination);
            }
        }

        public class FtpCopyWorkflowData
        {
            public LinkedServiceEntity Source { get; set; }

            public LinkedServiceEntity Destination { get; set; }
        }
    }
}
