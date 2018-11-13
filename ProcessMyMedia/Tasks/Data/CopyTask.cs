namespace ProcessMyMedia.Tasks
{
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Copy Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.TaskBase" />
    public class CopyTask : DataFactoryTaskBase
    {
        public Dataset InputDataset { get; set; }

        public Dataset OutputDataset { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyTask" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public CopyTask(IDataFactoryService service, ILoggerFactory loggerFactory) : base(service, loggerFactory)
        {

        }

        protected override Task Cleanup(IStepExecutionContext context)
        {
            throw new System.NotImplementedException();
        }


        protected override Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override void ValidateInput()
        {
            throw new System.NotImplementedException();
        }
    }
}
