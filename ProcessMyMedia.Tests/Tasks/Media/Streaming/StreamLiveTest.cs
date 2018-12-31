namespace ProcessMyMedia.Tests.Tasks.Media.Streaming
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    [TestClass]
    [TestCategory("Streaming")]
    public class StreamLiveTest :
         UnitTestBase<StreamLiveTest.StreamLiveWorkflow, StreamLiveTest.StreamLiveWorkflowData>
    {
        public StreamLiveTest()
        {
            this.Setup();
        }

        [TestMethod]
        public void SreamLiveWithoutNameTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();

            var datas = new StreamLiveWorkflowData();

            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.IsNull(this.GetData(workflowId).StreamingUrls);
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.StreamLiveTask.LiveEventName)));

            this.mediaService.Verify();
        }

        [TestMethod]
        public void StreamLiveOkTest()
        {
            IEnumerable<string> streamingUrls = new List<string>() { "http://url1", "http://url2" };

            Model.LiveEventEntity liveEvent = new Model.LiveEventEntity()
            {
                LiveEventName = "MyEvent",
                IngestUrls = new List<string>()
                {
                    "http://ingest.url"
                }
            };

            var datas = new StreamLiveWorkflowData()
            {
                AssetName = "MyAsset",
                LiveEventName = liveEvent.LiveEventName
            };

            this.mediaService.Setup(mock => mock.CreateLiveEventAsync(
                 It.Is<string>(s => s == datas.LiveEventName), It.Is<string>(s => s == datas.AssetName)))
                .Returns(Task.FromResult(liveEvent))
                .Verifiable();

            this.mediaService.Setup(mock => mock.CreateStreamingLocatorAsync(
                It.IsAny<string>(), It.Is<string>(s => s == datas.AssetName), It.IsAny<Model.StreamingOptions>()))
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
            Assert.IsNotNull(this.GetData(workflowId).IngestUrls);
            Assert.AreEqual(streamingUrls.Count(), this.GetData(workflowId).StreamingUrls.Count);
            Assert.AreEqual(liveEvent.IngestUrls.Count(), this.GetData(workflowId).IngestUrls.Count);

            mediaService.Verify();
        }



        public class StreamLiveWorkflow : IWorkflow<StreamLiveWorkflowData>
        {
            public string Id => nameof(StreamLiveWorkflow);

            public int Version => 1;

            public void Build(IWorkflowBuilder<StreamLiveWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
                    .StartWith<ProcessMyMedia.Tasks.StreamLiveTask>()
                        .Input(task => task.AssetName, data => data.AssetName)
                        .Input(task => task.LiveEventName, data => data.LiveEventName)
                        .Output(data => data.StreamingUrls, task => task.Output.StreamingUrls)
                        .Output(data => data.IngestUrls, task => task.Output.IngestUrls)
                        .Output(data => data.LocatorName, task => task.Output.LocatorName);

            }
        }

        public class StreamLiveWorkflowData
        {
            public string LiveEventName { get; set; }

            public string AssetName { get; set; }

            public List<string> StreamingUrls { get; set; }

            public List<string> IngestUrls { get; set; }

            public string LocatorName { get; set; }
        }
    }
}
