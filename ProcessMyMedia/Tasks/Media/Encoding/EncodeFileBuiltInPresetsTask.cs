namespace ProcessMyMedia.Tasks
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.Extensions.Logging;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;
    using ProcessMyMedia.Services.Contract;

    /// <summary>
    /// Encode File BuiltInPresets Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.EncodeFileTaskBase" />
    public class EncodeFileBuiltInPresetsTask : EncodeFileTaskBase
    {
        private List<BuiltInPreset> buildInPresets;

        /// <summary>
        /// Gets or sets the presets.
        /// </summary>
        /// <value>
        /// The presets.
        /// </value>
        public List<string> Presets { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodeFileBuiltInPresetsTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public EncodeFileBuiltInPresetsTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService,
         loggerFactory)
        {
            this.Presets = new List<string>();
            this.buildInPresets = new List<BuiltInPreset>();
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <exception cref="ArgumentException">AssetName</exception>
        protected override void ValidateInput()
        {
            base.ValidateInput();

            if (this.Presets.Count == 0)
            {
                throw new ArgumentException($"{nameof(this.Presets)} is empty");
            }

            foreach (var preset in this.Presets)
            {
                if (!Enum.TryParse<BuiltInPreset>(preset, out BuiltInPreset buildInPreset))
                {
                    throw new ArgumentException($"{preset} is not a valid preset for {nameof(this.Presets)} argument");
                }

                this.buildInPresets.Add(buildInPreset);
            }
         
        }

        /// <summary>
        /// Runs the media encoding task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected async override Task RunMediaEncodingTaskAsync(IStepExecutionContext context)
        {
            this.Outputs.AddRange(this.buildInPresets.Select(preset => new BuiltInPresetEncodingOutput(preset)));

            await base.RunMediaEncodingTaskAsync(context);
        }
    }
}
