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
    public class GenericCopyTask : DataFactoryTaskBase<GenericCopyTaskOutput>
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
            DataPipelineRunEntity run = context.PersistenceData as DataPipelineRunEntity;

            string runID;
            if (run == null)
            {
                //First call : Create and run the pipeline
                var inputDataset = this.SourcePath.ToDatasetEntity();
                var outputDataset = this.DestinationPath.ToDatasetEntity();

                await this.service.CreateOrUpdateDatasetAsync(inputDataset);
                await this.service.CreateOrUpdateDatasetAsync(outputDataset);

                DataPipelineEntity pipeline = new DataPipelineEntity()
                {
                    Name = Guid.NewGuid().ToString(),
                    Description = "Generic Copy pipeline",
                    Activities =
                    {
                        new CopyActivityEntity()
                        {
                            Name = nameof(GenericCopyTask),
                            Type = "Copy",
                            Source = this.SourcePath,
                            Destination = this.DestinationPath,
                            InputDatasetName = inputDataset.Name,
                            OutputDatasetName = outputDataset.Name
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
                if(!string.IsNullOrEmpty(run.InputDatasetName))
                {
                    await this.service.DeleteDatasetAsync(run.InputDatasetName);
                }
                if(!string.IsNullOrEmpty(run.OutputDatasetName))
                {
                    await this.service.DeleteDatasetAsync(run.OutputDatasetName);
                }            
            }
        }

    }
}
