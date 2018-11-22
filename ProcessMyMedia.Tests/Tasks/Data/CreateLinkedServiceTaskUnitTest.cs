namespace ProcessMyMedia.Tests.Tasks.Data
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using Moq;

    using ProcessMyMedia.Model;

    [TestClass]
    [TestCategory("DataFactory")]
    public class CreateLinkedServiceTaskUnitTest 
        : UnitTestBase<CreateLinkedServiceTaskUnitTest.CreateLinkedServiceWorkflow, CreateLinkedServiceTaskUnitTest.CreateLinkedServiceWorkflowData>
    {
        public CreateLinkedServiceTaskUnitTest()
        {
            this.Setup();
        }

        [TestMethod]
        public void CreateLinkedServiceWithoutArgumentTest()
        {
            this.dataFactoryService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.dataFactoryService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();


            var datas = new CreateLinkedServiceWorkflowData();


            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.CreateLinkedServiceTask.LinkedServiceToCreate)));
        }

        [TestMethod]
        public void CreateLinkedServiceWithoutNameTest()
        {
            this.dataFactoryService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.dataFactoryService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();


            var datas = new CreateLinkedServiceWorkflowData()
            {
                Server = new LinkedServiceEntity()
            };


            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.CreateLinkedServiceTask.LinkedServiceToCreate)));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.CreateLinkedServiceTask.LinkedServiceToCreate.Name)));
        }

        [TestMethod]
        public void CreateLinkedServiceWithoutTypeTest()
        {
            this.dataFactoryService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.dataFactoryService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();


            var datas = new CreateLinkedServiceWorkflowData()
            {
                Server = new LinkedServiceEntity()
                {
                    Name = "MyServer"
                }
            };


            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.CreateLinkedServiceTask.LinkedServiceToCreate)));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.CreateLinkedServiceTask.LinkedServiceToCreate.Type)));
        }

        [TestMethod]
        public void CreateLinkedServiceTest()
        {
            this.dataFactoryService.Setup(mock => mock.AuthAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.dataFactoryService.Setup(mock => mock.Dispose()).Verifiable();


            var datas = new CreateLinkedServiceWorkflowData()
            {
                Server = new LinkedServiceEntity()
                {
                    Name = "MyServer",
                    Type = "FtpServer",
                    TypeProperties = new
                    {
                        host = "localhost",
                        port = 21
                    }
                }
            };

            this.dataFactoryService.Setup(mock => mock.CreateOrUpdateLinkedServiceAsync(
                It.Is<string>(s => s == datas.Server.Name),
                It.Is<string>(s => s == datas.Server.Type),
                It.IsAny<object>()))
                .Returns(Task.CompletedTask)
                .Verifiable();


            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Complete, this.GetStatus((workflowId)));

            this.dataFactoryService.Verify();
        }

        public class CreateLinkedServiceWorkflow : IWorkflow<CreateLinkedServiceWorkflowData>
        {
            public string Id => nameof(CreateLinkedServiceWorkflow);

            public int Version => 1;

            public void Build(IWorkflowBuilder<CreateLinkedServiceWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowCore.Models.WorkflowErrorHandling.Terminate)
                    .StartWith<ProcessMyMedia.Tasks.CreateLinkedServiceTask>()
                    .Input(task => task.LinkedServiceToCreate, data => data.Server);
            }
        }

        public class CreateLinkedServiceWorkflowData
        {
            public LinkedServiceEntity Server { get; set; }
        }
    }
}
