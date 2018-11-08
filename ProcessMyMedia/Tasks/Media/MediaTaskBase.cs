namespace ProcessMyMedia.Tasks
{
    using System.Threading.Tasks;

    using WorkflowCore.Interface;

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
    public abstract class MediaTaskBase : TaskBase, ITask
    {
        protected IMediaService mediaService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTaskBase"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public MediaTaskBase(IMediaService mediaService, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            this.mediaService = mediaService;
        }

        protected async override Task Initialize(IStepExecutionContext context)
        {
            await this.mediaService.AuthAsync();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            this.mediaService.Dispose();
        }
    }
}
