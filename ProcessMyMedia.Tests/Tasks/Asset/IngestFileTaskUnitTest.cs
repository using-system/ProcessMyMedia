namespace ProcessMyMedia.Tests.Tasks.Asset
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    [TestClass]
    [TestCategory("Asset")]
    public class IngestFileTaskUnitTest : UnitTestBase<IngestFileTaskUnitTest.IngestFileWorkflow2, IngestFileTaskUnitTest.IngestFileWorkflowData>
    {
        public IngestFileTaskUnitTest()
        {
            this.Setup();
        }

        [TestMethod]
        public void IngestFileWithoutInputTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<Exception>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<Exception>();

            var workflowId = this.StartWorkflow(new IngestFileWorkflowData());
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
        }

        public class IngestFileWorkflow2: IWorkflow<IngestFileWorkflowData>
        {
            public string Id => nameof(IngestFileWorkflow2);

            public int Version => 1;

            public void Build(IWorkflowBuilder<IngestFileWorkflowData> builder)
            {
                builder
                    .StartWith<ProcessMyMedia.Tasks.IngestFileTask>()
                    .Input(task => task.AssetFilePath, data => data.File)
                    .OnError(WorkflowErrorHandling.Terminate);
            }
        }

        public class IngestFileWorkflowData
        {
            public string File { get; set; }
        }
    }
}
