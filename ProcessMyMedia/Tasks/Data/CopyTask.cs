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
    public class CopyTask : TaskBase
    {
        protected IDataService dataService;

        public Dataset InputDataset { get; set; }

        public Dataset OutputDataset { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyTask"/> class.
        /// </summary>
        /// <param name="dataService">The data service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public CopyTask(IDataService dataService, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.dataService = dataService;
        }

        protected override Task Cleanup(IStepExecutionContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override Task Initialize(IStepExecutionContext context)
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
