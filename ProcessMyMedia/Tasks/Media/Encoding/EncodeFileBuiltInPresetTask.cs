namespace ProcessMyMedia.Tasks.Media.Encoding
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Encode File BuiltInPreset Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.EncodeFileTaskBase" />
    public class EncodeFileBuiltInPresetTask : EncodeFileTaskBase
    {
        private BuiltInPreset buildInPreset;

        /// <summary>
        /// Gets or sets the preset.
        /// </summary>
        /// <value>
        /// The preset.
        /// </value>
        public string Preset { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodeFileBuiltInPresetTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public EncodeFileBuiltInPresetTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService,
         loggerFactory)
        {

        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <exception cref="ArgumentException">AssetName</exception>
        public override void ValidateInput()
        {
            if (string.IsNullOrEmpty(this.Preset))
            {
                throw new ArgumentException($"{nameof(this.Preset)} is required");
            }

            if(!Enum.TryParse<BuiltInPreset>(this.Preset, out this.buildInPreset))
            {
                throw new ArgumentException($"{this.Preset} is not a valid preset for {nameof(this.Preset)} argument");
            }
        }

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context)
        {
            this.Outputs.Add(new BuiltInPresetEncodingOutput(this.buildInPreset));

            return base.RunMediaTaskAsync(context);
        }
    }
}
