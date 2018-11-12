namespace ProcessMyMedia.Tests.Tasks.Media.Encoding
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    [TestClass]
    [TestCategory("Encoding")]
    public class EncodeFileBuiltInPresetTasksUnitTest : 
        UnitTestBase<EncodeFileBuiltInPresetTasksUnitTest.EncodeFileBuiltInPresetsWorkflow, EncodeFileBuiltInPresetTasksUnitTest.EncodeFileBuiltInPresetsWorkflowData>
    {
        public EncodeFileBuiltInPresetTasksUnitTest()
        {
            this.Setup();
        }

        public class EncodeFileBuiltInPresetsWorkflow : IWorkflow<EncodeFileBuiltInPresetsWorkflowData>
        {
            public string Id => nameof(EncodeFileBuiltInPresetsWorkflow);

            public int Version => 1;

            public void Build(IWorkflowBuilder<EncodeFileBuiltInPresetsWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
                    .StartWith(context => ExecutionResult.Next())
                    .If(data => data.Presets.Count == 1)
                    .Do(then =>
                        then.StartWith<ProcessMyMedia.Tasks.EncodeFileBuiltInPresetTask>()
                            .Input(task => task.FilePath, data => data.FilePath)
                            .Input(task => task.Preset, data => data.Presets.Single())
                            .Output(data => data.OutputAssetName, task => task.Output.Job.Outputs.First().Name))
                    .If(data => data.Presets.Count > 1)
                    .Do(then =>
                        then.StartWith<ProcessMyMedia.Tasks.EncodeFileBuiltInPresetsTask>()
                            .Input(task => task.FilePath, data => data.FilePath)
                            .Input(task => task.Presets, data => data.Presets)
                            .Output(data => data.OutputAssetName, task => task.Output.Job.Outputs.First().Name));
            }
        }

        public class EncodeFileBuiltInPresetsWorkflowData
        {
            public EncodeFileBuiltInPresetsWorkflowData()
            {
                this.Presets = new List<string>();
            }

            public List<string> Presets { get; set; }

            public string FilePath { get; set; }

            public string OutputAssetName {get; set; }

        }
    }
}
