namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Azure.Management.Media;
    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;

    /// <summary>
    /// Delete Job Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.MediaTaskBase" />
    public class DeleteJobTask : MediaTaskBase
    {
        /// <summary>
        /// Gets or sets the name of the job.
        /// </summary>
        /// <value>
        /// The name of the job.
        /// </value>
        public string TransformName { get; set; }

        /// <summary>
        /// Gets or sets the name of the job.
        /// </summary>
        /// <value>
        /// The name of the job.
        /// </value>
        public string JobName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteJobTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public DeleteJobTask(MediaConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {

        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        public override void ValidateInput()
        {
            if (string.IsNullOrEmpty(this.TransformName))
            {
                throw new ArgumentException($"{nameof(this.TransformName)} is required");
            }

            if (string.IsNullOrEmpty(this.JobName))
            {
                throw new ArgumentException($"{nameof(this.JobName)} is required");
            }
        }

        /// <summary>
        /// Runs the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override async Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context, AzureMediaServicesClient client)
        {
            await client.Jobs.DeleteAsync(this.configuration.ResourceGroup,
                this.configuration.MediaAccountName,
                this.TransformName,
                this.JobName);

            return ExecutionResult.Next();
        }
    }
}
