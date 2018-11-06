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
    public abstract class AnalyzeMediaTaskBase : MediaTaskBase<AnalyzeTaskOutput>
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
        public AnalyzeMediaTaskBase(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService, loggerFactory)
        {
            this.AnalyzingParameters = new AnalyzingParameters();
            this.CleanupResources = true;
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <exception cref="ArgumentException">AssetName</exception>
        public override void ValidateInput()
        {
            if (string.IsNullOrEmpty(this.AssetName))
            {
                throw new ArgumentException($"{nameof(this.AssetName)} is required");
            }
        }

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override async Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context)
        {
            JobEntity job = context.PersistenceData as JobEntity;

            if (job == null)
            {
                //First call: stat analyse
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

            foreach (var assetToDelete in job.OutputAssetNames)
            {
                await this.mediaService.DeleteAssetAsync(assetToDelete);
            }
        }
    }
}
