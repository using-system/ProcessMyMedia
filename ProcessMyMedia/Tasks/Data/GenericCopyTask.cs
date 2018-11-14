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

        protected override void ValidateInput()
        {
            throw new System.NotImplementedException();
        }


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
                            Properties =
                            {
                                { "type", "Copy" },
                                { "inputs" , new { name = this.DatasetInput.LinkedServiceName } },
                                { "outputs" , new { name = this.DatasetOutput.LinkedServiceName } }
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

        protected override Task Cleanup(IStepExecutionContext context)
        {
            throw new System.NotImplementedException();
        }


    }
}
