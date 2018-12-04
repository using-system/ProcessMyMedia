namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    public abstract class TaskBase<TService> : StepBodyAsync, ITask where TService : Services.Contract.IAzureService
    {
        protected ILogger logger;

        protected bool onError;

        protected TService service;

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
        /// <param name="service">The service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public TaskBase(TService service, ILoggerFactory loggerFactory)
        {
            this.service = service;
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

            await this.service.AuthAsync();

            try
            {
                this.logger.LogDebug($"Task {this.GetType().Name} started");

                var result = await this.RunTaskAsync(context);

                if (this.CleanupResources && !result.SleepFor.HasValue)
                {
                    await this.Cleanup(context);
                }

                this.logger.LogDebug($"Task {this.GetType().Name} completed");

                return result;
            }
            catch(Exception exc)
            {
                this.onError = true;

                this.logger.LogError($"Task {this.GetType().Name} terminated with error {exc}");

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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            this.service?.Dispose();
        }
    }
}
