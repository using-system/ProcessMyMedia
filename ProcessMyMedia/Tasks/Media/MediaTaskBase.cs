namespace ProcessMyMedia.Tasks
{
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
        /// Initializes a new instance of the <see cref="MediaTaskBase{T}" /> class.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="loggerFactory">The logger factory.</param>
        public MediaTaskBase(IMediaService service, ILoggerFactory loggerFactory)  :base(service, loggerFactory)
        {
            this.Output = new T();
        }
    }


    /// <summary>
    /// Media Task Base
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase" />
    public abstract class MediaTaskBase : TaskBase<IMediaService>, ITask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaTaskBase"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public MediaTaskBase(IMediaService service, ILoggerFactory loggerFactory) : base(service, loggerFactory)
        {

        }
    }
}
