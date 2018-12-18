namespace ProcessMyMedia.Tests.Tasks.Data
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;
    [TestClass]
    [TestCategory("DataFactory")]
    public class DeleteLinkedServiceTaskUnitTest
        : UnitTestBase<DeleteLinkedServiceTaskUnitTest.DeleteLinkedServiceWorkflow, DeleteLinkedServiceTaskUnitTest.DeleteLinkedServiceWorkflowData>
    {
        public DeleteLinkedServiceTaskUnitTest()
        {
            this.Setup();
        }


        [TestMethod]
        public void DeleteLinkedServiceWithoutNameTest()
        {
            this.dataFactoryService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.dataFactoryService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();


            var datas = new DeleteLinkedServiceWorkflowData();


            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.DeleteLinkedServiceTask.Name)));
        }

        [TestMethod]
        public void DeleteLinkedServiceTest()
        {
            this.dataFactoryService.Setup(mock => mock.AuthAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.dataFactoryService.Setup(mock => mock.Dispose()).Verifiable();


            var datas = new DeleteLinkedServiceWorkflowData()
            {
                Name = "LinkedServiceToDelete"
            };

            this.dataFactoryService.Setup(mock => mock.DeletLinkedServiceAsync(
                    It.Is<string>(s => s == datas.Name)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Complete, this.GetStatus((workflowId)));

            this.dataFactoryService.Verify();
        }

        public class DeleteLinkedServiceWorkflow : IWorkflow<DeleteLinkedServiceWorkflowData>
        {
            public string Id => nameof(DeleteLinkedServiceWorkflow);

            public int Version => 1;

            public void Build(IWorkflowBuilder<DeleteLinkedServiceWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowCore.Models.WorkflowErrorHandling.Terminate)
                    .StartWith<ProcessMyMedia.Tasks.DeleteLinkedServiceTask>()
                    .Input(task => task.Name, data => data.Name);
            }
        }

        public class DeleteLinkedServiceWorkflowData
        {
            public string Name { get; set; }
        }
    }
}
