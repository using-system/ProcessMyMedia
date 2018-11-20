namespace ProcessMyMedia.Tests.Tasks.Data
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WorkflowCore.Interface;

    using ProcessMyMedia.Model;

    [TestClass]
    [TestCategory("DataFactory")]
    public class CopyTaskUnitTest : UnitTestBase<CopyTaskUnitTest.CopyTaskWorkflow, CopyTaskUnitTest.CopyTaskWorkflowData>
    {
        public CopyTaskUnitTest()
        {
            this.Setup();
        }

        public class CopyTaskWorkflow : IWorkflow<CopyTaskWorkflowData>
        {
            public string Id => nameof(CopyTaskWorkflow);

            public int Version => 1;

            public void Build(IWorkflowBuilder<CopyTaskWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowCore.Models.WorkflowErrorHandling.Terminate)
                    .StartWith<ProcessMyMedia.Tasks.CopyTask>()
                        .Input(task => task.SourcePath, data => data.SourcePath)
                        .Input(task => task.DestinationPath, data => data.DestinationPath);
            }
        }

        public class CopyTaskWorkflowData
        {
            public DataPath SourcePath { get; set; }

            public DataPath DestinationPath { get; set; }
        }

    }
}
