namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Encode media task base class
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase{ProcessMyMedia.Model.EncodeTaskOutput}" />
    public abstract class EncodeTaskBase : MediaTaskBase<EncodeTaskOutput>
    {
        /// <summary>
        /// The delay service
        /// </summary>
        private IDelayService delayService;

        /// <summary>
        /// Gets or sets the asset names.
        /// </summary>
        /// <value>
        /// The asset names.
        /// </value>
        protected List<JobInputEntity> Inputs { get; set; }

        /// <summary>
        /// Gets or sets the outputs.
        /// </summary>
        /// <value>
        /// The outputs.
        /// </value>
        protected List<EncodingOutputBase> Outputs { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public JobPriority Priority { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodeTaskBase" /> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="delayService">The delay service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public EncodeTaskBase(IMediaService mediaService, IDelayService delayService, ILoggerFactory loggerFactory) : base(mediaService,
            loggerFactory)
        {
            this.delayService = delayService;
            this.CleanupResources = true;
            this.Inputs = new List<JobInputEntity>();
            this.Outputs = new List<EncodingOutputBase>();
            this.Priority = JobPriority.Normal;
        }

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Encoding Job was canceled</exception>
        protected override async Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context)
        {
            JobEntity job = context.PersistenceData as JobEntity;

            if (job == null)
            {
                //First call: start encoding
                await this.RunMediaEncodingTaskAsync(context);

                this.logger.LogInformation("Start encoding");

                job = await this.service.StartEncodeAsync(this.Inputs, this.Outputs, this.Priority);
            }
            else
            {
                job = await this.service.GetJobAsync(job.Name, job.TemplateName);
            }

            this.Output.Job = job;

            if (!job.IsFinished)
            {
                this.logger.LogInformation($"Encoding progress : {job.Progress} %");
                return ExecutionResult.Sleep(this.delayService.GetTimeToSleep(job.Created), job);
            }
            else if (job.Canceled)
            {
                throw new Exception("Encoding Job was canceled");
            }
            else if (job.OnError)
            {
                throw new Exception($"Encoding Job is on error : {job.ErrorMessage}");
            }

            return ExecutionResult.Next();
        }

        /// <summary>
        /// Runs the media encoding task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected abstract Task RunMediaEncodingTaskAsync(IStepExecutionContext context);

        /// <summary>
        /// Cleanups the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected async override Task Cleanup(IStepExecutionContext context)
        {
            JobEntity job = context.PersistenceData as JobEntity;

            if (job == null)
            {
                return;
            }

            await this.service.DeleteJobAsync(job.Name, job.TemplateName);
            await this.service.DeleteTemplateAsync(job.TemplateName);
        }

    }
}
