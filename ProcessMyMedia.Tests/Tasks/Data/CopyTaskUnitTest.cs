namespace ProcessMyMedia.Tests.Tasks.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using Moq;

    using ProcessMyMedia.Model;

    [TestClass]
    [TestCategory("DataFactory")]
    public class CopyTaskUnitTest : UnitTestBase<CopyTaskUnitTest.CopyTaskWorkflow, CopyTaskUnitTest.CopyTaskWorkflowData>
    {
        public CopyTaskUnitTest()
        {
            this.Setup();
        }


        [TestMethod]
        public void CopyWithoutSourceTest()
        {
            this.dataFactoryService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.dataFactoryService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();


            var datas = new CopyTaskWorkflowData();


            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.CopyTask.SourcePath)));
        }

        [TestMethod]
        public void CopyWithoutSourceServiceNameTest()
        {
            this.dataFactoryService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.dataFactoryService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();


            var datas = new CopyTaskWorkflowData()
            {
                SourcePath = new GenericDataPath()
            };


            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.CopyTask.SourcePath)));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.CopyTask.SourcePath.LinkedServiceName)));
        }


        [TestMethod]
        public void CopyWithoutDestinationTest()
        {
            this.dataFactoryService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.dataFactoryService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();


            var datas = new CopyTaskWorkflowData()
            {
                SourcePath = new GenericDataPath() {LinkedServiceName = "MySource"}
            };


            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.CopyTask.DestinationPath)));
        }

        [TestMethod]
        public void CopyWithoutDestinationServiceNameTest()
        {
            this.dataFactoryService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.dataFactoryService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();


            var datas = new CopyTaskWorkflowData()
            {
                SourcePath = new GenericDataPath() { LinkedServiceName = "MySource" },
                DestinationPath = new GenericDataPath()
            };


            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.CopyTask.DestinationPath)));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.CopyTask.DestinationPath.LinkedServiceName)));
        }

        [TestMethod]
        public void CopyTest()
        {

            var datas = new CopyTaskWorkflowData()
            {
                CleanUp = false,
                SourcePath = new GenericDataPath() { LinkedServiceName = "MySource" },
                DestinationPath = new GenericDataPath() { LinkedServiceName = "MyDestination" }
            };

            this.MockCopyTask(datas);


            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Complete, this.GetStatus((workflowId)));

            this.dataFactoryService.Verify();
        }

        [TestMethod]
        public void CopyTestWithCleanup()
        {

            var datas = new CopyTaskWorkflowData()
            {
                CleanUp = true,
                SourcePath = new GenericDataPath() { LinkedServiceName = "MySource" },
                DestinationPath = new GenericDataPath() { LinkedServiceName = "MyDestination" }
            };

            this.MockCopyTask(datas);


            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Complete, this.GetStatus((workflowId)));

            this.dataFactoryService.Verify();
        }


        [TestMethod]
        public void CopyTestOnError()
        {

            var datas = new CopyTaskWorkflowData()
            {
                CleanUp = true,
                SourcePath = new GenericDataPath() { LinkedServiceName = "MySource" },
                DestinationPath = new GenericDataPath() { LinkedServiceName = "MyDestination" }
            };

            this.MockCopyTask(datas, onError:true);


            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));

            this.dataFactoryService.Verify();
        }


        private void MockCopyTask(CopyTaskWorkflowData datas, bool onError = false)
        {
            this.dataFactoryService.Setup(mock => mock.AuthAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.dataFactoryService.Setup(mock => mock.Dispose()).Verifiable();
      

            string runID = Guid.NewGuid().ToString();
            string pipelineName = Guid.NewGuid().ToString();

            this.dataFactoryService.Setup(mock => mock.GetLinkedServiceAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new LinkedServiceEntity() { Type = LinkedServiceType.FileServer.ToString()}))
                .Verifiable();

            this.dataFactoryService.Setup(mock => mock.CreateOrUpdateDatasetAsync(
                    It.Is<DatasetEntity>(dataset => dataset.LinkedServiceName == datas.SourcePath.LinkedServiceName)))
                .Returns(Task.FromResult(new DatasetEntity()))
                .Verifiable();

            this.dataFactoryService.Setup(mock => mock.CreateOrUpdateDatasetAsync(
                    It.Is<DatasetEntity>(dataset => dataset.LinkedServiceName == datas.DestinationPath.LinkedServiceName)))
                .Returns(Task.FromResult(new DatasetEntity()))
                .Verifiable();

            this.dataFactoryService.Setup(mock => mock.CreateOrUpdatePipelineyAsync(
                    It.Is<DataPipelineEntity>(pipeline => true)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            this.dataFactoryService.Setup(mock => mock.RunPipelineAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()))
                .Returns(Task.FromResult(runID))
                .Verifiable();

            this.dataFactoryService.Setup(mock => mock.GetPipelineRunAsync(
                    It.Is<string>(id => id == runID)))
                .Returns(Task.FromResult(new DataPipelineRunEntity()
                {
                    ID = runID,
                    PipelineName = pipelineName,
                    OnError = onError,
                    IsFinished = true,
                    StartDate = DateTime.Now.AddMinutes(-5),
                    EndDate = DateTime.Now
                }))
                .Verifiable();

            if (datas.CleanUp)
            {
                this.dataFactoryService.Setup(mock => mock.DeletePipelineAsync(
                        It.Is<string>(name => name == pipelineName)))
                    .Returns(Task.FromResult(Task.CompletedTask))
                    .Verifiable();

                this.dataFactoryService.Setup(mock => mock.DeleteDatasetAsync(
                        It.IsAny<string>()))
                    .Returns(Task.FromResult(Task.CompletedTask))
                    .Verifiable();
            }
            else
            {
                this.dataFactoryService.Setup(mock => mock.DeletePipelineAsync(
                        It.IsAny<string>()))
                    .Throws<Exception>();

                this.dataFactoryService.Setup(mock => mock.DeleteDatasetAsync(
                        It.IsAny<string>()))
                    .Throws<Exception>();
            }


        }

        public class CopyTaskWorkflow : IWorkflow<CopyTaskWorkflowData>
        {
            public string Id => nameof(CopyTaskWorkflow);

            public int Version => 1;

            public void Build(IWorkflowBuilder<CopyTaskWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowCore.Models.WorkflowErrorHandling.Terminate)
                    .StartWith<ProcessMyMedia.Tasks.CopyTask>()
                        .Input(task => task.CleanupResources, data => data.CleanUp)
                        .Input(task => task.SourcePath, data => data.SourcePath)
                        .Input(task => task.DestinationPath, data => data.DestinationPath);
            }
        }


        public class CopyTaskWorkflowData
        {
            public bool CleanUp { get; set; }

            public DataPath SourcePath { get; set; }

            public DataPath DestinationPath { get; set; }
        }

    }
}
