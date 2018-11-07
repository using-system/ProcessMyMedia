using Microsoft.AspNetCore.Http;

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
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase{ProcessMyMedia.Model.AnalyzeTaskOutput}" />
    public abstract class AnalyzeTaskBase : MediaTaskBase<AnalyzeTaskOutput>
    {
        /***
         * https://docs.microsoft.com/en-us/azure/media-services/latest/analyze-videos-tutorial-with-api
         * https://github.com/Azure-Samples/media-services-v3-dotnet-tutorials/tree/master/AMSV3Tutorials/AnalyzeVideos
         * https://docs.microsoft.com/en-us/azure/media-services/latest/analyzing-video-audio-files-concept
         */
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
        /// Gets or sets a value indicating whether [cleanup resources].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cleanup resources]; otherwise, <c>false</c>.
        /// </value>
        public bool CleanupResources { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaAnalyzerTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public AnalyzeTaskBase(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
        {
            this.AnalyzingParameters = new AnalyzingParameters();
            this.CleanupResources = true;
        }

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override async Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context)
        {
            JobEntity job = context.PersistenceData as JobEntity;

            try
            {
                if (job == null)
                {
                    //First call: stat analyse
                    await this.RunMediaAnalyseTaskAsync(context);
                    job = await this.mediaService.StartAnalyseAsync(this.AssetName, this.AnalyzingParameters);
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
                    throw new Exception("Analysing Job was canceled");
                }

                this.Output.Result = await this.mediaService.EndAnalyseAsync(job);

                await this.Cleanup(job);
            }
            catch
            {
                await this.Cleanup(job);
                throw;
            }

            return ExecutionResult.Next();
        }

        /// <summary>
        /// Runs the media analyse task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected abstract Task RunMediaAnalyseTaskAsync(IStepExecutionContext context);

        /// <summary>
        /// Cleanups the specified job.
        /// </summary>
        /// <param name="job">The job.</param>
        /// <returns></returns>
        protected async virtual Task Cleanup(JobEntity job)
        {
            var jobToDelete = await this.mediaService.GetJobAsync(job.Name, job.TemplateName);
            if (jobToDelete != null)
            {
                await this.mediaService.DeleteJobAsync(job.Name, job.TemplateName);
            }

            var templateToDelete = await this.mediaService.GetTemplateAsync(job.TemplateName);
            if (templateToDelete != null)
            {
                await this.mediaService.DeleteTemplateAsync(job.TemplateName);
            }
        }
    }
}
