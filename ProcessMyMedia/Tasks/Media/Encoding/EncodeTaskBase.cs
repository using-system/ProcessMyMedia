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
        /// Gets or sets the asset names.
        /// </summary>
        /// <value>
        /// The asset names.
        /// </value>
        protected List<string> AssetNames { get; set; }

        /// <summary>
        /// Gets or sets the outputs.
        /// </summary>
        /// <value>
        /// The outputs.
        /// </value>
        protected List<EncodingOutputBase> Outputs { get; set; }

        public bool CleanupResources { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodeTaskBase"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public EncodeTaskBase(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService,
            loggerFactory)
        {
            this.AssetNames = new List<string>();
            this.Outputs = new List<EncodingOutputBase>();
        }

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Encoding Job was canceled</exception>
        public override async Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context)
        {
            JobEntity job = context.PersistenceData as JobEntity;

            if (job == null)
            {
                //First call: stat encoding
                job = await this.mediaService.StartEncodeAsync(this.AssetNames, this.Outputs);
            }
            else
            {
                job = await this.mediaService.GetJobAsync(job.Name, job.TemplateName);
            }

            this.Output.Job = job;

            if (!job.IsFinished)
            {
                return ExecutionResult.Sleep(this.GetTimeToSleep(job.Created), job);
            }
            else if (job.Canceled)
            {
                throw new Exception("Encoding Job was canceled");
            }

            if (this.CleanupResources)
            {
                await this.Cleanup(job);
            }

            return ExecutionResult.Next();
        }

        /// <summary>
        /// Cleanups the specified job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        protected async virtual Task Cleanup(JobEntity job)
        {
            await this.mediaService.DeleteJobAsync(job.Name, job.TemplateName);

            await this.mediaService.DeleteTemplateAsync(job.TemplateName);
        }

    }
}
