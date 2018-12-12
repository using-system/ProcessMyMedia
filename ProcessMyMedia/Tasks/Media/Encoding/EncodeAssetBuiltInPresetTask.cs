namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;

    public class EncodeAssetBuiltInPresetTask : EncodeTaskBase
    {
        private BuiltInPreset buildInPreset;

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public JobInputEntity Input { get; set; }

        /// <summary>
        /// Gets or sets the preset.
        /// </summary>
        /// <value>
        /// The preset.
        /// </value>
        public string Preset { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodeAssetBuiltInPresetTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="delayService">The delay service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public EncodeAssetBuiltInPresetTask(IMediaService mediaService, IDelayService delayService, ILoggerFactory loggerFactory)
            : base(mediaService, delayService, loggerFactory)
        {
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// </exception>
        protected override void ValidateInput()
        {
            if (this.Input == null)
            {
                throw new ArgumentException($"{nameof(this.Input)} is required");
            }

            if (string.IsNullOrEmpty(this.Preset))
            {
                throw new ArgumentException($"{nameof(this.Preset)} is required");
            }

            if (!Enum.TryParse<BuiltInPreset>(this.Preset, out this.buildInPreset))
            {
                throw new ArgumentException($"{this.Preset} is not a valid preset for {nameof(this.Preset)} argument");
            }
        }

        /// <summary>
        /// Runs the media encoding task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override Task RunMediaEncodingTaskAsync(IStepExecutionContext context)
        {
            base.Inputs.Add(this.Input);
            this.Outputs.Add(new BuiltInPresetEncodingOutput(this.buildInPreset));

            return Task.CompletedTask;
        }


    }
}
