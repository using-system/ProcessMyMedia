namespace ProcessMyMedia.Tasks
{
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
        where T : class   
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

        }
    }


    /// <summary>
    /// Media Task Base
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase" />
    public abstract class MediaTaskBase : StepBodyAsync, IMediaTask
    {
        protected IMediaService mediaService;

        protected ILogger logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTaskBase"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public MediaTaskBase(IMediaService mediaService, ILoggerFactory loggerFactory)
        {
            this.mediaService = mediaService;
            this.logger = loggerFactory.CreateLogger(this.GetType());
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
                return await this.RunMediaTaskAsync(context);
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
        public abstract Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}
