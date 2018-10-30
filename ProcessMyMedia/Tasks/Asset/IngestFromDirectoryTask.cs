﻿using System;

namespace ProcessMyMedia.Tasks
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Microsoft.Azure.Management.Media;

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
        /// Initializes a new instance of the <see cref="IngestFromDirectoryTask" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory"></param>
        public IngestFromDirectoryTask(WamsConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
            this.TopDirectoryOnly = true;
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <exception cref="ArgumentException">AssetDirectoryPath</exception>
        public override void ValidateInput()
        {
            base.ValidateInput();

            if (string.IsNullOrEmpty(this.AssetDirectoryPath))
            {
                throw new ArgumentException($"{nameof(this.AssetDirectoryPath)} is required");
            }
        }

        /// <summary>
        /// Runs the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="client">The client.</param>
        /// <returns></returns>
        public override Task<ExecutionResult> RunMediaTaskAsync(IStepExecutionContext context, AzureMediaServicesClient client)
        {
            if (string.IsNullOrEmpty(this.SearchPattern))
            {
                this.SearchPattern = "*.*";
            }

            this.AssetFiles.AddRange(Directory.GetFiles(this.AssetDirectoryPath, 
                this.SearchPattern, 
                this.TopDirectoryOnly? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories));

            return base.RunMediaTaskAsync(context, client);
        }
    }
}
