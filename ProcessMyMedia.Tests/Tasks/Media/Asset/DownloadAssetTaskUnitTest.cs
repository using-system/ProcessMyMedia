namespace ProcessMyMedia.Tests.Tasks.Media.Asset
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    [TestClass]
    [TestCategory("Asset")]
    public class DownloadAssetTaskUnitTest : UnitTestBase<DownloadAssetTaskUnitTest.DownloadAssetWorkfloww, DownloadAssetTaskUnitTest.DownloadAssetWorkflowwData>
    {
        public DownloadAssetTaskUnitTest()
        {
            this.Setup();
        }

        [TestMethod]
        public void DownloadAssetWithoutInputTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();

            var workflowId = this.StartWorkflow(new DownloadAssetWorkflowwData() );
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));

        }


        public class DownloadAssetWorkfloww : IWorkflow<DownloadAssetWorkflowwData>
        {
            public string Id => nameof(DownloadAssetWorkfloww);

            public int Version => 1;

            public void Build(IWorkflowBuilder<DownloadAssetWorkflowwData> builder)
            {
                builder
                    .StartWith<ProcessMyMedia.Tasks.DownloadAssetTask>()
                        .Input(task => task.AssetName, data => data.AssetName)
                        .Input(task => task.DirectoryToDownload, data => data.DirectoryToDownload)
                    .OnError(WorkflowErrorHandling.Terminate);
            }
        }

        public class DownloadAssetWorkflowwData
        {
            public string AssetName { get; set; }

            public string DirectoryToDownload { get; set; }
        }
    }
}
