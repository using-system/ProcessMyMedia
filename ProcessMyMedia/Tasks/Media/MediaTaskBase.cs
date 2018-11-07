namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using Microsoft.Extensions.Logging;

    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Media Task Base
    /// </summary>
    /// <typeparam name="T">Output class</typeparam>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="WorkflowCore.Models.StepBodyAsync" />
    public abstract class MediaTaskBase<T> : MediaTaskBase
        where T : class, new()   
    {
        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public T Output { get; protected set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTaskBase{T}"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public MediaTaskBase(IMediaService mediaService, ILoggerFactory loggerFactory)  :base(mediaService, loggerFactory)
        {
            this.Output = new T();
        }
    }


    /// <summary>
    /// Media Task Base
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase" />
    public abstract class MediaTaskBase : StepBodyAsync, ITask
    {
        protected IMediaService mediaService;

        protected ILogger logger;

        protected bool onError;

        /// <summary>
        /// Gets or sets a value indicating whether [cleanup resources].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cleanup resources]; otherwise, <c>false</c>.
        /// </value>
        public bool CleanupResources { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTaskBase"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public MediaTaskBase(IMediaService mediaService, ILoggerFactory loggerFactory)
        {
            this.mediaService = mediaService;
            this.logger = loggerFactory.CreateLogger(this.GetType());
            this.CleanupResources = true;
            this.onError = false;
        }

        /// <summary>
        /// Runs the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            this.ValidateInput();

            await this.mediaService.AuthAsync();

            try
            {
                var result = await this.RunMediaTaskAsync(context);

                if (this.CleanupResources)
                {
                    await this.Cleanup(context);
                }

                return result;
            }
            catch
            {
                this.onError = true;

                if (this.CleanupResources)
                {
                    await this.Cleanup(context);
                }

                throw;
            }
            finally
            {
                this.mediaService.Dispose();
            }
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        public abstract void ValidateInput();

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected abstract Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context);

        /// <summary>
        /// Cleanups the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected abstract Task Cleanup(IStepExecutionContext context);

        /// <summary>
        /// Gets the time to sleep.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <returns></returns>
        protected TimeSpan GetTimeToSleep(DateTime startDate)
        {
            double totalSeconds = (DateTime.Now - startDate.ToLocalTime()).TotalSeconds;

            if (totalSeconds < 60)
            {
                return TimeSpan.FromSeconds(5);
            }
            else if(totalSeconds < 60 * 5)
            {
                return TimeSpan.FromSeconds(30);
            }
            else if (totalSeconds < 60 * 30)
            {
               return TimeSpan.FromSeconds(60);
            }
            else
            {
                return TimeSpan.FromMinutes(3);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}
