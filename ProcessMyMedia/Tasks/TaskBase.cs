namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    public abstract class TaskBase : StepBodyAsync, ITask
    {
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
        /// Initializes a new instance of the <see cref="MediaTaskBase" /> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        public TaskBase(ILoggerFactory loggerFactory)
        {
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

            await this.Initialize(context);

            try
            {
                var result = await this.RunTaskAsync(context);

                if (this.CleanupResources && !result.SleepFor.HasValue)
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
                this.Dispose();
            }
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        protected abstract void ValidateInput();

        /// <summary>
        /// Initializes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected abstract Task Initialize(IStepExecutionContext context);

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected abstract Task<ExecutionResult> RunTaskAsync(IStepExecutionContext context);

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
            else if (totalSeconds < 60 * 5)
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
