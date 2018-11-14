namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Generic Copy Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.TaskBase" />
    public class GenericCopyTask : DataFactoryTaskBase
    {
        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public DatasetEntity Input { get; set; }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public DatasetEntity Output { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericCopyTask" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public GenericCopyTask(IDataFactoryService service, ILoggerFactory loggerFactory) : base(service, loggerFactory)
        {

        }

        protected override void ValidateInput()
        {
            throw new System.NotImplementedException();
        }


        protected async override Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context)
        {
            await this.service.CreateOrUpdateDatasetAsync(this.Input);
            await this.service.CreateOrUpdateDatasetAsync(this.Output);

            DataPipelineEntity pipeline = new DataPipelineEntity()
            {
                Name = Guid.NewGuid().ToString(),
                Description = "Generic Copy pipeline",
                Activities =
                {
                    new DataActivityEntity()
                    {
                        Name = nameof(GenericCopyTask),
                        Properties =
                        {
                            { "type", "Copy" },
                            { "inputs" , new { name = this.Input.LinkedServiceName } },
                            { "outputs" , new { name = this.Output.LinkedServiceName } }
                        }
                    }
                }
            };

            await this.service.CreateOrUpdatePipelineyAsync(pipeline);

            return ExecutionResult.Next();

        }

        protected override Task Cleanup(IStepExecutionContext context)
        {
            throw new System.NotImplementedException();
        }


    }
}
