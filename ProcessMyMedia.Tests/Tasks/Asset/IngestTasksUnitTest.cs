namespace ProcessMyMedia.Tests.Tasks.Asset
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;

    [TestClass]
    [TestCategory("Asset")]
    public class IngestTasksUnitTest : UnitTestBase<IngestTasksUnitTest.IngestWorkflow, IngestTasksUnitTest.IngestWorkflowData>
    {
        public IngestTasksUnitTest()
        {
            this.Setup();
        }

        [TestMethod]
        public void IngestFileWithoutInputTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<Exception>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<Exception>();

            var workflowId = this.StartWorkflow(new IngestWorkflowData(){FilePath = "c:\test.mpg"}); //asset name missing
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(Guid.Empty, this.GetData(workflowId).AssetID);

        }

        [TestMethod]
        public void IngestFileTest()
        {
            AssetEntity expected = new AssetEntity()
            {
                AssetID = Guid.NewGuid()
            };

            var workflowDatas = new IngestWorkflowData()
            {
                AssetName = "MyAsset",
                FilePath = "c:\test.mpg"
            };

            this.mediaService.Setup(mock => mock.CreateOrUpdateAssetAsync(
                    It.Is<string>(s => s == "MyAsset"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns( () => Task.FromResult(expected))
                .Verifiable();
            this.mediaService.Setup(mock => mock.UploadFilesToAssetAsync(
                    It.Is<string>(s => s == "MyAsset"), 
                    It.Is<IEnumerable<string>>(files => files.Count() == 1 && files.Contains(workflowDatas.FilePath)), 
                    It.Is<IDictionary<string, string>>(metadata => metadata.Count() == 0)))
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.mediaService.Setup(mock => mock.AuthAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.mediaService.Setup(mock => mock.Dispose()).Verifiable();


            var workflowId = this.StartWorkflow(workflowDatas);

            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Complete, this.GetStatus((workflowId)));
            Assert.AreEqual(expected.AssetID, this.GetData(workflowId).AssetID);

            mediaService.Verify();
        }

        public class IngestWorkflow: IWorkflow<IngestWorkflowData>
        {
            public string Id => nameof(IngestWorkflow);

            public int Version => 1;

            public void Build(IWorkflowBuilder<IngestWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
                    .StartWith(context => ExecutionResult.Next())
                    .If(data => !string.IsNullOrEmpty(data.FilePath))
                    .Do(then =>
                        then.StartWith<ProcessMyMedia.Tasks.IngestFileTask>()
                            .Input(task => task.AssetFilePath, data => data.FilePath)
                            .Input(task => task.AssetName, data => data.AssetName)
                            .Output(data => data.AssetID, task=> task.Output.Asset.AssetID));
            }
        }

        public class IngestWorkflowData
        {
            public string AssetName { get; set; }

            public string FilePath { get; set; }

            public Guid AssetID { get; set; }
        }
    }
}
