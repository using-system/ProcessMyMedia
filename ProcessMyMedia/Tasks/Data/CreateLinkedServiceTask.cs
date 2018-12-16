namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;

    public class CreateLinkedServiceTask : DataFactoryTaskBase<LinkedServiceEntity>
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public object Properties { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateLinkedServiceTask"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public CreateLinkedServiceTask(IDataFactoryService service, ILoggerFactory loggerFactory) : base(service, loggerFactory)
        {

        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// LinkedServiceToCreate
        /// or
        /// Name
        /// or
        /// Type
        /// </exception>
        protected override void ValidateInput()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                throw new ArgumentException($"{nameof(this.Name)} is required");
            }

            if (string.IsNullOrEmpty(this.Type))
            {
                throw new ArgumentException($"{nameof(this.Type)} is required");
            }
        }


        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected async override Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context)
        {
            this.logger.LogInformation($"Create the Linked service {this.Name}");

            this.Output = await this.service.CreateOrUpdateLinkedServiceAsync(
                this.Name,
                this.Type,
                this.Properties);

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
