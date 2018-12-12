using System.Linq;

namespace ProcessMyMedia.Tests.Tasks.Media.Streaming
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    [TestClass]
    public class StreamTest :
        UnitTestBase<StreamTest.StreamAssetWorkflow, StreamTest.StreamAssetWorkflowData>
    {
        public StreamTest()
        {
            this.Setup();
        }

        [TestMethod]
        public void PublicStreamWithoutAssetTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();

            var datas = new StreamAssetWorkflowData();

            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.IsNull(this.GetData(workflowId).StreamingUrls);
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.StreamTask.AssetName)));

            this.mediaService.Verify();
        }

        [TestMethod]
        public void IngestFilesTest()
        {
            IEnumerable<string> streamingUrls = new List<string>(){"http://url1", "http://url2" };

            var datas = new StreamAssetWorkflowData()
            {
                AssetName = "MyAsset"
            };

            this.mediaService.Setup(mock => mock.CreateStreamingLocatorAsync(
                It.IsAny<string>(), It.Is<string>(s => s == datas.AssetName)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            this.mediaService.Setup(mock => mock.GetStreamingUrlsAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(streamingUrls))
                .Verifiable();

            this.mediaService.Setup(mock => mock.AuthAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.mediaService.Setup(mock => mock.Dispose()).Verifiable();


            var workflowId = this.StartWorkflow(datas);

            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Complete, this.GetStatus((workflowId)));
            Assert.IsNotNull(this.GetData(workflowId).StreamingUrls);
            Assert.IsNotNull(this.GetData(workflowId).LocatorName);
            Assert.AreEqual(streamingUrls.Count(), this.GetData(workflowId).StreamingUrls.Count);

            mediaService.Verify();
        }

        public class StreamAssetWorkflow : IWorkflow<StreamAssetWorkflowData>
        {
            public string Id => nameof(StreamAssetWorkflow);

            public int Version => 1;

            public void Build(IWorkflowBuilder<StreamAssetWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
                    .StartWith<ProcessMyMedia.Tasks.StreamTask>()
                        .Input(task => task.AssetName, data => data.AssetName)
                        .Output(data => data.StreamingUrls, task => task.Output.StreamingUrls)
                        .Output(data => data.LocatorName, task => task.Output.LocatorName);

            }
        }

        public class StreamAssetWorkflowData
        { 
            public string AssetName { get; set; }

            public List<string> StreamingUrls { get; set; }

            public string LocatorName { get; set; }
        }
    }
}
