namespace ProcessMyMedia.Tasks
{
    using Microsoft.Extensions.Logging;

    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Data Generic Task base class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ProcessMyMedia.Tasks.Data.DataTaskBase" />
    public abstract class DataFactoryTaskBase<T> : DataFactoryTaskBase
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
        /// Initializes a new instance of the <see cref="DataFactoryTaskBase{T}"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public DataFactoryTaskBase(IDataFactoryService service, ILoggerFactory loggerFactory) : base(service, loggerFactory)
        {
            this.Output = new T();
        }
    }


    /// <summary>
    /// Data Task Base class
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase" />
    public abstract class DataFactoryTaskBase : TaskBase<IDataFactoryService>, IDataFactoryTask
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataFactoryTaskBase"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public DataFactoryTaskBase(IDataFactoryService service, ILoggerFactory loggerFactory) : base(service, loggerFactory)
        {

        }
    }
}
