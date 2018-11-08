namespace ProcessMyMedia.Tasks.Media.Encoding
{
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
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.EncodeTaskBase" />
    public class EncodeAssetsTask : EncodeTaskBase
    {
        /// <summary>
        /// Gets or sets the asset names.
        /// </summary>
        /// <value>
        /// The asset names.
        /// </value>
        public new List<JobInputEntity> Inputs { get; set; }

        /// <summary>
        /// Gets or sets the outputs.
        /// </summary>
        /// <value>
        /// The outputs.
        /// </value>
        public new List<CustomPresetEncodingOutput> Outputs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodeAssetsTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public EncodeAssetsTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService,
            loggerFactory)
        {
            this.Inputs = new List<JobInputEntity>();
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        public override void ValidateInput()
        {
            throw new System.NotImplementedException();
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
                base.Outputs.AddRange(this.Outputs);
            });

        }
    }
}
