namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Services.Contract;
    using ProcessMyMedia.Model;

    /// <summary>
    /// Analyse task base class
    /// https://docs.microsoft.com/en-us/azure/media-services/latest/analyze-videos-tutorial-with-api
    /// https://github.com/Azure-Samples/media-services-v3-dotnet-tutorials/tree/master/AMSV3Tutorials/AnalyzeVideos
    /// https://docs.microsoft.com/en-us/azure/media-services/latest/analyzing-video-audio-files-concept
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase{ProcessMyMedia.Model.AnalyzeTaskOutput}" />
    public abstract class AnalyzeTaskBase : MediaTaskBase<AnalyzeTaskOutput>
    {
        private IDelayService delayService;

        /// <summary>
        /// Gets or sets the name of the asset.
        /// </summary>
        /// <value>
        /// The name of the asset.
        /// </value>
        protected string AssetName { get; set; }

        /// <summary>
        /// Gets or sets the analyzing parameters.
        /// </summary>
        /// <value>
        /// The analyzing parameters.
        /// </value>
        public AnalyzingParameters AnalyzingParameters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAnalyzerTask" /> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="delayService">The delay service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public AnalyzeTaskBase(IMediaService mediaService, IDelayService delayService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
        {
            this.delayService = delayService;
            this.AnalyzingParameters = new AnalyzingParameters();
        }

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override async Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context)
        {
            JobEntity job = context.PersistenceData as JobEntity;

            if (job == null)
            {
                //First call: stat analyse
                await this.RunMediaAnalyseTaskAsync(context);

                this.logger.LogInformation($"Start analyse the asset {this.AssetName}");

                job = await this.service.StartAnalyseAsync(this.AssetName, this.AnalyzingParameters);
            }
            else
            {
                job = await this.service.GetJobAsync(job.Name, job.TemplateName);
            }

            this.Output.Job = job;

            if (!job.IsFinished)
            {
                this.logger.LogInformation($"Analysing progress : {job.Progress} %");

                return ExecutionResult.Sleep(this.delayService.GetTimeToSleep(job.Created), job);
            }
            else if (job.Canceled)
            {
                throw new Exception("Analysing Job was canceled");
            }

            this.Output.Result = await this.service.EndAnalyseAsync(job);


            return ExecutionResult.Next();
        }

        /// <summary>
        /// Runs the media analyse task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected abstract Task RunMediaAnalyseTaskAsync(IStepExecutionContext context);

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
