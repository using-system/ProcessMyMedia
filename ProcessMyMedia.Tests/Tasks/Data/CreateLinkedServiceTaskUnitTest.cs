namespace ProcessMyMedia.Tests.Tasks.Data
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WorkflowCore.Interface;

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
