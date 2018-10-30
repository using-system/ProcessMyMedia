namespace ProcessMyMedia.Tests.Tasks.Asset
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;
    using WorkflowCore.Testing;

    [TestClass]
    public class DeleteAssetTaskTest : WorkflowTest<DeleteAssetTaskTest.TestWorfklow, DeleteAssetTaskTest.TestWorkflowData>
    {
        public DeleteAssetTaskTest()
        {
            this.Setup();
        }

        [TestMethod]
        public void  DeleteAssetWithoutAssetNameTest()
        {
            var workflowId = this.StartWorkflow(new TestWorkflowData());
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
        }

        public class TestWorfklow : IWorkflow<TestWorkflowData>
        {
            public string Id => nameof(TestWorfklow);

            public int Version => 1;

            public void Build(IWorkflowBuilder<TestWorkflowData> builder)
            {
                builder
                    .StartWith<ProcessMyMedia.Tasks.DeleteAssetTask>()
                    .Input(task => task.AssetName, data => data.AssetNameToDelete);
            }
        }

        public class TestWorkflowData
        {
            public string AssetNameToDelete { get; set; }
        }
    }
}
