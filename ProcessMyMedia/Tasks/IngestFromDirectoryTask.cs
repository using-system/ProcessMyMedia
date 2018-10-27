namespace ProcessMyMedia.Tasks
{
    using System.IO;
    using System.Threading.Tasks;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;

    /// <summary>
    /// Ingest From Directory Task
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Tasks.IngestTaskBase" />
    public class IngestFromDirectoryTask : IngestTaskBase
    {
        /// <summary>
        /// Gets or sets the asset directory path.
        /// </summary>
        /// <value>
        /// The asset directory path.
        /// </value>
        public string AssetDirectoryPath { get; set; }

        /// <summary>
        /// Gets or sets the search pattern.
        /// </summary>
        /// <value>
        /// The search pattern.
        /// </value>
        public string SearchPattern { get; set; }

        /// <summary>
        /// Specifies of the ingest task get files only on the top directory.
        /// Default value : true
        /// </summary>
        /// <value>
        ///   <c>true</c> if [top directory only]; otherwise, <c>false</c>.
        /// </value>
        public bool TopDirectoryOnly { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IngestFromDirectoryTask"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public IngestFromDirectoryTask(MediaConfiguration configuration) : base(configuration)
        {
            this.TopDirectoryOnly = true;
        }

        public override Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context)
        {
            this.AssetFiles.AddRange(Directory.GetFiles(this.AssetDirectoryPath, 
                this.SearchPattern, 
                this.TopDirectoryOnly? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories));

            return base.RunMediaTaskAsync(context);
        }
    }
}
