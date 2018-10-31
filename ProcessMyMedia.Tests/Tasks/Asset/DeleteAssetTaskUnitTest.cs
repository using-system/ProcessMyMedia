namespace ProcessMyMedia.Tests.Tasks.Asset
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    [TestClass]
    [TestCategory("Asset")]
    public class DeleteAssetTaskUnitTest : UnitTestBase<DeleteAssetTaskUnitTest.TestWorfklow, DeleteAssetTaskUnitTest.TestWorkflowData>
    {
        public DeleteAssetTaskUnitTest()
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

        [TestMethod]
        public void DeleteAssetTest()
        {
            var workflowId = this.StartWorkflow(new TestWorkflowData()
            {
                AssetNameToDelete = "MyAsset"
            });

            this.mediaService.Setup(mock => mock.DeleteAssetAsync(It.Is<string>(s => s == "MyAsset")))
                .Returns(() =>
                {
                    return Task.CompletedTask;
                })
                .Verifiable();
            this.mediaService.Setup(mock => mock.Dispose()).Verifiable();

            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));



            Assert.AreEqual(WorkflowStatus.Complete, this.GetStatus((workflowId)));
            
            this.mediaService.Verify();
        }

        public class TestWorfklow : IWorkflow<TestWorkflowData>
        {
            public string Id => nameof(TestWorfklow);

            public int Version => 1;

            public void Build(IWorkflowBuilder<TestWorkflowData> builder)
            {
                builder
                    .StartWith<ProcessMyMedia.Tasks.DeleteAssetTask>()
                    .Input(task => task.AssetName, data => data.AssetNameToDelete)
                    .OnError(WorkflowErrorHandling.Terminate);
            }
        }

        public class TestWorkflowData
        {
            public string AssetNameToDelete { get; set; }
        }
    }
}
