namespace ProcessMyMedia.Tests.Tasks.Media.Asset
{
    using System;
    using System.IO;
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
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();

            var workflowId = this.StartWorkflow(new IngestWorkflowData(){FilePath = "c:\test.mpg"}); //asset name missing
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(null, this.GetData(workflowId).AssetID);
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.IngestFileTask.AssetName)));

        }

        [TestMethod]
        public void IngestFileTest()
        {
            AssetEntity expected = new AssetEntity()
            {
                AssetID = Guid.NewGuid().ToString()
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

        [TestMethod]
        public void IngestFilesWithoutInputTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();

            var workflowId = this.StartWorkflow(new IngestWorkflowData()
            {
                Files =
                {
                    "c:\test1.mpg",
                    "c:\test2.mpg",
                    "c:\test3.mpg"
                }
            }); //asset name missing
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(null, this.GetData(workflowId).AssetID);
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.IngestFilesTask.AssetName)));

        }


        [TestMethod]
        public void IngestFilesTest()
        {
            AssetEntity expected = new AssetEntity()
            {
                AssetID = Guid.NewGuid().ToString()
            };

            var workflowDatas = new IngestWorkflowData()
            {
                AssetName = "MyAsset",
                Files =
                {
                    "c:\test1.mpg",
                    "c:\test2.mpg",
                    "c:\test3.mpg"
                }
            };

            this.mediaService.Setup(mock => mock.CreateOrUpdateAssetAsync(
                    It.Is<string>(s => s == "MyAsset"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => Task.FromResult(expected))
                .Verifiable();
            this.mediaService.Setup(mock => mock.UploadFilesToAssetAsync(
                    It.Is<string>(s => s == "MyAsset"),
                    It.Is<IEnumerable<string>>(files => files.Count() == 3),
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

        [TestMethod]
        public void IngestFromDirectoryWithoutInputTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();

            var workflowId = this.StartWorkflow(new IngestWorkflowData()
            {
                Directory = Directory.GetCurrentDirectory()
            }); //asset name missing
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(null, this.GetData(workflowId).AssetID);
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.IngestFromDirectoryTask.AssetName)));

        }

        [TestMethod]
        public void IngestFromDirectoryTest()
        {
            AssetEntity expected = new AssetEntity()
            {
                AssetID = Guid.NewGuid().ToString()
            };

            var workflowDatas = new IngestWorkflowData()
            {
                AssetName = "MyAsset",
                Directory = Directory.GetCurrentDirectory()
            };

            this.mediaService.Setup(mock => mock.CreateOrUpdateAssetAsync(
                    It.Is<string>(s => s == "MyAsset"), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() => Task.FromResult(expected))
                .Verifiable();
            this.mediaService.Setup(mock => mock.UploadFilesToAssetAsync(
                    It.Is<string>(s => s == "MyAsset"),
                    It.Is<IEnumerable<string>>(files => files.Count() > 0),
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
                            .Output(data => data.AssetID, task => task.Output.Asset.AssetID))
                    .If(data => data.Files.Count > 0)
                    .Do(then =>
                        then.StartWith<ProcessMyMedia.Tasks.IngestFilesTask>()
                            .Input(task => task.AssetFiles, data => data.Files)
                            .Input(task => task.AssetName, data => data.AssetName)
                            .Output(data => data.AssetID, task => task.Output.Asset.AssetID))
                    .If(data => !string.IsNullOrEmpty(data.Directory))
                    .Do(then =>
                        then.StartWith<ProcessMyMedia.Tasks.IngestFromDirectoryTask>()
                            .Input(task => task.AssetDirectoryPath, data => data.Directory)
                            .Input(task => task.AssetName, data => data.AssetName)
                            .Output(data => data.AssetID, task => task.Output.Asset.AssetID));
            }
        }

        public class IngestWorkflowData
        {
            public IngestWorkflowData()
            {
                this.Files = new List<string>();
            }

            public string AssetName { get; set; }

            public string FilePath { get; set; }

            public List<string> Files { get; set; }

            public string Directory { get; set; }

            public string AssetID { get; set; }
        }
    }
}
