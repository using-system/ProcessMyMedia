namespace ProcessMyMedia.Tests.Tasks.Media.Asset
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    [TestClass]
    [TestCategory("Asset")]
    public class DeleteAssetTaskUnitTest : UnitTestBase<DeleteAssetTaskUnitTest.DeleteAssetWorkflow, DeleteAssetTaskUnitTest.DeleteAssetWorkflowData>
    {
        public DeleteAssetTaskUnitTest()
        {
            this.Setup();
        }

        [TestMethod]
        public void  DeleteAssetWithoutAssetNameTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();

            var workflowId = this.StartWorkflow(new DeleteAssetWorkflowData());
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
        }

        [TestMethod]
        public void DeleteAssetTest()
        {
            this.mediaService.Setup(mock => mock.DeleteAssetAsync(It.Is<string>(s => s == "MyAsset")))
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.mediaService.Setup(mock => mock.AuthAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.mediaService.Setup(mock => mock.Dispose()).Verifiable();


            var workflowId = this.StartWorkflow(new DeleteAssetWorkflowData()
            {
                AssetNameToDelete = "MyAsset"
            });

            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));



            Assert.AreEqual(WorkflowStatus.Complete, this.GetStatus((workflowId)));
            
           mediaService.Verify();
        }

        [TestMethod]
        public void DeleteAssetKoTest()
        {
            this.mediaService.Setup(mock => mock.DeleteAssetAsync(It.Is<string>(s => s == "MyAsset")))
                .Throws<Exception>();
            this.mediaService.Setup(mock => mock.AuthAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.mediaService.Setup(mock => mock.Dispose()).Verifiable();


            var workflowId = this.StartWorkflow(new DeleteAssetWorkflowData()
            {
                AssetNameToDelete = "MyAsset"
            });

            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));



            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));

            mediaService.Verify();
        }

        public class DeleteAssetWorkflow : IWorkflow<DeleteAssetWorkflowData>
        {
            public string Id => nameof(DeleteAssetWorkflow);

            public int Version => 1;

            public void Build(IWorkflowBuilder<DeleteAssetWorkflowData> builder)
            {
                builder
                    .StartWith<ProcessMyMedia.Tasks.DeleteAssetTask>()
                    .Input(task => task.AssetName, data => data.AssetNameToDelete)
                    .OnError(WorkflowErrorHandling.Terminate);
            }
        }

        public class DeleteAssetWorkflowData
        {
            public string AssetNameToDelete { get; set; }
        }
    }
}
