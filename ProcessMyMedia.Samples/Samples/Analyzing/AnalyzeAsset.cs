namespace ProcessMyMedia.Samples
{
    using System;
    using System.IO;

    using Microsoft.Extensions.Configuration;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    public class AnalyseAsset : WofkflowSampleBase<AnalyseAsset.AnalyzeAssetWorkflow, AnalyseAsset.AnalyzeAssetWorkflowData>
    {
        public AnalyseAsset(IConfigurationRoot configuration) : base(configuration)
        {

        }

        protected override AnalyzeAssetWorkflowData WorflowDatas => new AnalyzeAssetWorkflowData()
        {
            InputAssetName = "AnalyzeAsset",
            MediaDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Asset2"),
            DirectoryToDownload = Path.Combine(Directory.GetCurrentDirectory(), "output/", Guid.NewGuid().ToString())
        };

        public class AnalyzeAssetWorkflow : IWorkflow<AnalyzeAssetWorkflowData>
        {
            public string Id => SampleBase.WORKFLOW_NAME;

            public int Version => 1;

            public void Build(IWorkflowBuilder<AnalyzeAssetWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
                    .StartWith<Tasks.IngestFromDirectoryTask>()
                        .Input(task => task.AssetDirectoryPath, data => data.MediaDirectory)
                        .Input(task => task.AssetName, data => data.InputAssetName)
                    .Then<Tasks.AnalyzeAssetTask>()
                        .Input(task => task.AssetName, data => data.InputAssetName)
                        .Output(data => data.OutputAssetName, task => task.Output.Result.OutputAssetName)
                    .Then<Tasks.DeleteAssetTask>()
                        .Input(task => task.AssetName, data => data.InputAssetName)
                    .If(data => !string.IsNullOrEmpty(data.OutputAssetName))
                    .Do(then =>
                        //Get the analysing result
                        then.StartWith<Tasks.DownloadAssetTask>()
                            .Input(task => task.AssetName, data => data.OutputAssetName)
                            .Input(task => task.DirectoryToDownload, data => data.DirectoryToDownload)
                        //Delete output asset (analysing result)
                        .Then< Tasks.DeleteAssetTask>()
                            .Input(task => task.AssetName, data => data.OutputAssetName));
            }
        }

        public class AnalyzeAssetWorkflowData
        {
            public string MediaDirectory { get; set; }

            public string InputAssetName { get; set; }

            public string OutputAssetName { get; set; }

            public string DirectoryToDownload { get; set; }
        }
    }
}
