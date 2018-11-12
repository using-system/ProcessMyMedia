namespace ProcessMyMedia.Tests.Tasks.Media.Asset
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

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
        public void DownloadAssetWithoutAssetNameTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();

            var workflowId = this.StartWorkflow(new DownloadAssetWorkflowwData() );
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.DownloadAssetTask.AssetName)));
        }

        [TestMethod]
        public void DownloadAssetWithoutDirectoryTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();

            var workflowId = this.StartWorkflow(new DownloadAssetWorkflowwData(){AssetName = "MyAsset"});
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.DownloadAssetTask.DirectoryToDownload)));
        }

        [TestMethod]
        public void DownloadAssetTest()
        {
            var datas = new DownloadAssetWorkflowwData()
            {
                AssetName = "MyAsset",
                DirectoryToDownload = Directory.GetCurrentDirectory()
            };

            this.mediaService.Setup(mock => mock.DownloadFilesAsync(
                    It.Is<string>(s => s == datas.AssetName), It.Is<string>(s => s == datas.DirectoryToDownload)))
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.mediaService.Setup(mock => mock.AuthAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.mediaService.Setup(mock => mock.Dispose()).Verifiable();


            var workflowId = this.StartWorkflow(datas);

            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));


            Assert.AreEqual(WorkflowStatus.Complete, this.GetStatus((workflowId)));

            mediaService.Verify();
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
