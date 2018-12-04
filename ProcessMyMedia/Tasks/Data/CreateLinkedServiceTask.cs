namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;

    public class CreateLinkedServiceTask : DataFactoryTaskBase
    {
        /// <summary>
        /// Gets or sets the linked service to create.
        /// </summary>
        /// <value>
        /// The linked service to create.
        /// </value>
        public LinkedServiceEntity LinkedServiceToCreate { get; set; }

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
            if (this.LinkedServiceToCreate == null)
            {
                throw new ArgumentException($"{nameof(this.LinkedServiceToCreate)} is required");
            }

            if (string.IsNullOrEmpty(this.LinkedServiceToCreate.Name))
            {
                throw new ArgumentException($"{nameof(this.LinkedServiceToCreate.Name)} is required for the property {nameof(this.LinkedServiceToCreate)}");
            }

            if (string.IsNullOrEmpty(this.LinkedServiceToCreate.Type))
            {
                throw new ArgumentException($"{nameof(this.LinkedServiceToCreate.Type)} is required for the property {nameof(this.LinkedServiceToCreate)}");
            }
        }


        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected async override Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context)
        {
            this.logger.LogInformation($"Create the Linked service {this.LinkedServiceToCreate.Name}");

            await this.service.CreateOrUpdateLinkedServiceAsync(
                this.LinkedServiceToCreate.Name,
                this.LinkedServiceToCreate.Type,
                this.LinkedServiceToCreate.TypeProperties);

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
