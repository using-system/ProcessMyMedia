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
    public class GenericCopyTask : DataFactoryTaskBase<GenericCopyTaskOutput>
    {
        private IDelayService delayService;

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public DatasetEntity DatasetInput { get; set; }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public DatasetEntity DatasetOutput { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericCopyTask" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="delayService">The delay service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public GenericCopyTask(IDataFactoryService service, IDelayService delayService, ILoggerFactory loggerFactory) : base(service, loggerFactory)
        {
            this.delayService = delayService;
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// DatasetInput
        /// or
        /// Name
        /// or
        /// LinkedServiceName
        /// or
        /// DatasetOutput
        /// or
        /// Name
        /// or
        /// LinkedServiceName
        /// </exception>
        protected override void ValidateInput()
        {
            if (this.DatasetInput == null)
            {
                throw new ArgumentException($"{nameof(this.DatasetInput)} is required");
            }

            if (string.IsNullOrEmpty(this.DatasetInput.Name))
            {
                throw new ArgumentException($"{nameof(this.DatasetInput.Name)} is required for the property {this.DatasetInput}");
            }

            if (string.IsNullOrEmpty(this.DatasetInput.LinkedServiceName))
            {
                throw new ArgumentException($"{nameof(this.DatasetInput.LinkedServiceName)} is required for the property {this.DatasetInput}");
            }

            if (this.DatasetOutput == null)
            {
                throw new ArgumentException($"{nameof(this.DatasetOutput)} is required");
            }

            if (string.IsNullOrEmpty(this.DatasetOutput.Name))
            {
                throw new ArgumentException($"{nameof(this.DatasetOutput.Name)} is required for the property {this.DatasetOutput}");
            }

            if (string.IsNullOrEmpty(this.DatasetOutput.LinkedServiceName))
            {
                throw new ArgumentException($"{nameof(this.DatasetOutput.LinkedServiceName)} is required for the property {this.DatasetOutput}");
            }
        }


        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected async override Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context)
        {
            DataPipelineRunEntity run = context.PersistenceData as DataPipelineRunEntity;

            string runID;
            if (run == null)
            {
                //First call : Create and run the pipeline

                await this.service.CreateOrUpdateDatasetAsync(this.DatasetInput);
                await this.service.CreateOrUpdateDatasetAsync(this.DatasetOutput);

                DataPipelineEntity pipeline = new DataPipelineEntity()
                {
                    Name = Guid.NewGuid().ToString(),
                    Description = "Generic Copy pipeline",
                    Activities =
                    {
                        new DataActivityEntity()
                        {
                            Name = nameof(GenericCopyTask),
                            Type = "Copy",
                            InputDatasetName = this.DatasetInput.Name,
                            OutputDatasetName = this.DatasetOutput.Name,
                            TypeProperties = new
                            {
  
                            }
                        }
                    }
                };

                await this.service.CreateOrUpdatePipelineyAsync(pipeline);

                runID = await this.service.RunPipelineAsync(pipeline.Name);
            }
            else
            {
                runID = run.ID;
            }

            run = await this.service.GetPipelineRunAsync(runID);

            this.Output.Run = run;

            if (!run.IsFinished)
            {
                return ExecutionResult.Sleep(this.delayService.GetTimeToSleep(run.StartDate), run);
            }
            else if (run.OnError)
            {
                throw new Exception($"Data Copy is on error : {run.ErrorMessage}");
            }


            return ExecutionResult.Next();

        }

        /// <summary>
        /// Cleanups the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected async override Task Cleanup(IStepExecutionContext context)
        {
            DataPipelineRunEntity run = context.PersistenceData as DataPipelineRunEntity;

            if (run != null)
            {
                await this.service.DeletePipelineAsync(run.PipelineName);
                await this.service.DeleteDatasetAsync(this.DatasetInput.Name);
                await this.service.DeleteDatasetAsync(this.DatasetOutput.Name);
            }
        }


    }
}
