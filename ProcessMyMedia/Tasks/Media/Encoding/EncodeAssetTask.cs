namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using WorkflowCore.Interface;

    using Microsoft.Extensions.Logging;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Encode Assets Task
    /// https://docs.microsoft.com/fr-fr/rest/api/media/transforms/createorupdate#standardencoderpreset
    /// https://github.com/MicrosoftDocs/azure-docs/blob/master/articles/media-services/previous/media-services-mes-presets-overview.md
    /// https://docs.microsoft.com/en-us/azure/media-services/previous/media-services-advanced-encoding-with-mes
    /// https://docs.microsoft.com/en-us/azure/media-services/latest/customize-encoder-presets-how-to
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.EncodeTaskBase" />
    public class EncodeAssetTask : EncodeTaskBase
    {

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public JobInputEntity Input { get; set; }

        /// <summary>
        /// Gets or sets the encoding output.
        /// </summary>
        /// <value>
        /// The encoding output.
        /// </value>
        public CustomPresetEncodingOutput EncodingOutput { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodeAssetTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public EncodeAssetTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService,
            loggerFactory)
        {
            this.Inputs = new List<JobInputEntity>();
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        public override void ValidateInput()
        {
            if(this.Input == null)
            {
                throw new ArgumentException($"{nameof(this.Input)} is required");
            }

            if (this.EncodingOutput == null)
            {
                throw new ArgumentException($"{nameof(this.Output)} is required");
            }

            this.Input.Validate();
            this.EncodingOutput.Validate();
        }

        /// <summary>
        /// Runs the media encoding task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected async override Task RunMediaEncodingTaskAsync(IStepExecutionContext context)
        {
            await Task.Run(() =>
            {
                base.Inputs = this.Inputs;
                base.Outputs.Add(this.EncodingOutput);
            });

        }
    }
}
