namespace ProcessMyMedia.Tasks
{
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
        /// <summary>
        /// Gets or sets the presets.
        /// </summary>
        /// <value>
        /// The presets.
        /// </value>
        public List<BuiltInPreset> Presets { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodeFileBuiltInPresetsTask"/> class.
        /// </summary>
        /// <param name="mediaService">The media service.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public EncodeFileBuiltInPresetsTask(IMediaService mediaService, ILoggerFactory loggerFactory) : base(mediaService,
         loggerFactory)
        {
            this.Presets = new List<BuiltInPreset>();
        }

        /// <summary>
        /// Runs the media task asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context)
        {
            this.Outputs.AddRange(this.Presets.Select(preset => new BuiltInPresetEncodingOutput(preset)));

            return base.RunMediaTaskAsync(context);
        }
    }
}
