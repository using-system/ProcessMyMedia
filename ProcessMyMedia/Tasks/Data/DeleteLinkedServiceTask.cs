namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Services.Contract;

    public class DeleteLinkedServiceTask : DataFactoryTaskBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteLinkedServiceTask"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public DeleteLinkedServiceTask(IDataFactoryService service, ILoggerFactory loggerFactory) : base(service, loggerFactory)
        {

        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        protected override void ValidateInput()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new ArgumentException($"{nameof(this.Name)} is required");
            }

        }

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override async Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context)
        {
            this.logger.LogInformation($"Delete the linked service {this.Name}");

            await this.service.DeletLinkedServiceAsync(this.Name);

            return ExecutionResult.Next();
        }

        /// <summary>
        /// Cleanups the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override Task Cleanup(IStepExecutionContext context)
        {
            return Task.CompletedTask;
        }


    }
}
