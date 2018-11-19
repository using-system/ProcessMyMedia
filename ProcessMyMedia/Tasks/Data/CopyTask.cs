namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Extensions;
    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Generic Copy Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.TaskBase" />
    public class CopyTask : DataFactoryTaskBase<CopyTaskOutput>
    {
        private IDelayService delayService;

        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        /// <value>
        /// The source path.
        /// </value>
        public DataPath SourcePath { get; set; }

        /// <summary>
        /// Gets or sets the destination path.
        /// </summary>
        /// <value>
        /// The destination path.
        /// </value>
        public DataPath DestinationPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CopyTask" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="delayService">The delay service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public CopyTask(IDataFactoryService service, IDelayService delayService, ILoggerFactory loggerFactory) : base(service, loggerFactory)
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
            if (this.SourcePath == null)
            {
                throw new ArgumentException($"{nameof(this.SourcePath)} is required");
            }

            if (string.IsNullOrEmpty(this.SourcePath.LinkedServiceName))
            {
                throw new ArgumentException($"{nameof(this.SourcePath.LinkedServiceName)} is required for the property {this.SourcePath}");
            }

            if (this.DestinationPath == null)
            {
                throw new ArgumentException($"{nameof(this.DestinationPath)} is required");
            }

            if (string.IsNullOrEmpty(this.DestinationPath.LinkedServiceName))
            {
                throw new ArgumentException($"{nameof(this.DestinationPath.LinkedServiceName)} is required for the property {this.DestinationPath}");
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
            this.Output = context.PersistenceData as CopyTaskOutput;

            string runID;
            if (this.Output == null)
            {
                //First call : Create and run the pipeline
                this.Output = new CopyTaskOutput();

                this.Output.InputDataset = this.SourcePath.ToDatasetEntity();
                this.Output.OutputDataset = this.DestinationPath.ToDatasetEntity();

                await this.service.CreateOrUpdateDatasetAsync(this.Output.InputDataset);
                await this.service.CreateOrUpdateDatasetAsync(this.Output.OutputDataset);

                DataPipelineEntity pipeline = new DataPipelineEntity()
                {
                    Name = Guid.NewGuid().ToString(),
                    Description = "Generic Copy pipeline",
                    Activities =
                    {
                        new CopyActivityEntity()
                        {
                            Name = nameof(CopyTask),
                            Type = "Copy",
                            Source = this.SourcePath,
                            Destination = this.DestinationPath,
                            InputDatasetName =  this.Output.InputDataset.Name,
                            OutputDatasetName =  this.Output.OutputDataset.Name
                        }
                    }
                };

                await this.service.CreateOrUpdatePipelineyAsync(pipeline);

                runID = await this.service.RunPipelineAsync(pipeline.Name);
            }
            else
            {
                runID = this.Output.Run.ID;
            }

            this.Output.Run = await this.service.GetPipelineRunAsync(runID);

            if (!this.Output.Run.IsFinished)
            {
                return ExecutionResult.Sleep(this.delayService.GetTimeToSleep(this.Output.Run.StartDate), this.Output);
            }
            else if (this.Output.Run.OnError)
            {
                throw new Exception($"Data Copy is on error : { this.Output.Run.ErrorMessage}");
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
            CopyTaskOutput taskOutput = context.PersistenceData as CopyTaskOutput;

            if (taskOutput != null)
            {
                await this.service.DeletePipelineAsync(taskOutput.Run.PipelineName);
                if(!string.IsNullOrEmpty(taskOutput.InputDataset?.Name))
                {
                    await this.service.DeleteDatasetAsync(taskOutput.InputDataset.Name);
                }
                if(!string.IsNullOrEmpty(taskOutput.OutputDataset?.Name))
                {
                    await this.service.DeleteDatasetAsync(taskOutput.OutputDataset.Name);
                }            
            }
        }

    }
}
