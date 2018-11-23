namespace ProcessMyMedia.Tests.Tasks.Media.Encoding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using ProcessMyMedia.Model;

    [TestClass]
    [TestCategory("Encoding")]
    public class EncodeFileBuiltInPresetTasksUnitTest : 
        UnitTestBase<EncodeFileBuiltInPresetTasksUnitTest.EncodeFileBuiltInPresetsWorkflow, EncodeFileBuiltInPresetTasksUnitTest.EncodeFileBuiltInPresetsWorkflowData>
    {
        public EncodeFileBuiltInPresetTasksUnitTest()
        {
            this.Setup();
        }

        [TestMethod]
        public void EncodeFileBuiltInPresetWithoutFilePathTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();

            var workflowId = this.StartWorkflow(new EncodeFileBuiltInPresetsWorkflowData() { Presets = { null } });
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.IsNull(this.GetData(workflowId).OutputAssetName);
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.EncodeFileBuiltInPresetTask.FilePath)));

        }

        [TestMethod]
        public void EncodeFileBuiltInPresetWithoutPresetNamePathTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();

            var workflowId = this.StartWorkflow(new EncodeFileBuiltInPresetsWorkflowData()
            {
                FilePath = Directory.GetFiles(Directory.GetCurrentDirectory()).First(),
                Presets = { null }
            });
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.IsNull(this.GetData(workflowId).OutputAssetName);
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.EncodeFileBuiltInPresetTask.Preset)));
        }

        [TestMethod]
        public void EncodeFileBuiltInPresetTest()
        {
            var workflowId = this.StartWorkflow(this.MockEncodingTask(new List<string>(){BuiltInPreset.H264MultipleBitrateSD.ToString()}));

            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Complete, this.GetStatus((workflowId)));
            Assert.IsFalse(string.IsNullOrEmpty(this.GetData(workflowId).OutputAssetName));

            mediaService.Verify();
        }

        [TestMethod]
        public void EncodeFileBuiltInPresetCancelledTest()
        {
            var workflowId = this.StartWorkflow(this.MockEncodingTask(new List<string>() { BuiltInPreset.H264MultipleBitrateSD.ToString() },
                cancelled:true));

            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.IsTrue(string.IsNullOrEmpty(this.GetData(workflowId).OutputAssetName));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(Exception));

            mediaService.Verify();
        }

        [TestMethod]
        public void EncodeFileBuiltInPresetOnErrorTest()
        {
            var workflowId = this.StartWorkflow(this.MockEncodingTask(new List<string>() { BuiltInPreset.H264MultipleBitrateSD.ToString() },
                onError: true));

            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.IsTrue(string.IsNullOrEmpty(this.GetData(workflowId).OutputAssetName));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(Exception));

            mediaService.Verify();
        }

        [TestMethod]
        public void EncodeFileBuiltInPresetWithoutCleanupTest()
        {
            var workflowId = this.StartWorkflow(this.MockEncodingTask(new List<string>() { BuiltInPreset.H264MultipleBitrateSD.ToString() },
                cleanUp: false));

            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Complete, this.GetStatus((workflowId)));
            Assert.IsFalse(string.IsNullOrEmpty(this.GetData(workflowId).OutputAssetName));

            mediaService.Verify();

        }



        [TestMethod]
        public void EncodeFileBuiltInPresetsWithoutFilePathTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();

            var workflowId = this.StartWorkflow(new EncodeFileBuiltInPresetsWorkflowData() { Presets = { null, null } });
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.IsTrue(string.IsNullOrEmpty(this.GetData(workflowId).OutputAssetName));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.EncodeFileBuiltInPresetTask.FilePath)));
        }

        [TestMethod]
        public void EncodeFileBuiltInPresetWithoutPresetsNamePathTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync()).Throws<NotSupportedException>();
            this.mediaService.Setup(mock => mock.Dispose()).Throws<NotSupportedException>();

            var workflowId = this.StartWorkflow(new EncodeFileBuiltInPresetsWorkflowData()
            {
                FilePath = Directory.GetFiles(Directory.GetCurrentDirectory()).First(),
                Presets = { null, null }
            });
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.IsTrue(string.IsNullOrEmpty(this.GetData(workflowId).OutputAssetName));
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.EncodeFileBuiltInPresetTask.Preset)));
        }

        [TestMethod]
        public void EncodeFileBuiltInPresetsTest()
        {
            var workflowId = this.StartWorkflow(this.MockEncodingTask(new List<string>()
            {
                BuiltInPreset.H264MultipleBitrateSD.ToString(),
                BuiltInPreset.H264SingleBitrate720p.ToString()
            }));

            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Complete, this.GetStatus((workflowId)));
            Assert.IsFalse(string.IsNullOrEmpty(this.GetData(workflowId).OutputAssetName));

            mediaService.Verify();
        }

        private EncodeFileBuiltInPresetsWorkflowData MockEncodingTask(List<string> presets,
            bool cancelled = false, 
            bool onError = false,
            bool cleanUp = true)
        {
            var datas = new EncodeFileBuiltInPresetsWorkflowData()
            {
                CleanUp = cleanUp,
                FilePath = Directory.GetFiles(Directory.GetCurrentDirectory()).First(),
                Presets = presets
            };

            var expected = new JobEntity()
            {
                Name = "Job1",
                TemplateName = "Template1",
                Created = DateTime.Now,
                Outputs =
                {
                    new JobOutputEntity(){ Name="Output1"}
                }
            };
            string inputAssetName = null;

            this.mediaService.Setup(mock => mock.CreateOrUpdateAssetAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string, string, string>((assetName, assetDescription, storageAccountName, containerName) =>
                {
                    inputAssetName = assetName;
                    expected.Inputs.Add(new JobInputEntity() { Name = inputAssetName });
                    return Task.FromResult(new AssetEntity() { Name = assetName });
                })
                .Verifiable();
            this.mediaService.Setup(mock => mock.UploadFilesToAssetAsync(
                    It.Is<string>(s => s == inputAssetName),
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<IDictionary<string, string>>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            this.mediaService.Setup(mock => mock.StartEncodeAsync(
                    It.Is<IEnumerable<JobInputEntity>>(s => s.Count() == 1 && s.First().Name == inputAssetName),
                    It.Is<IEnumerable<EncodingOutputBase>>(s => s.Count() == datas.Presets.Count
                        && s.First() is BuiltInPresetEncodingOutput
                        && ((BuiltInPresetEncodingOutput)s.First()).Preset.ToString() == datas.Presets.First()),
                    It.Is<JobPriority>(s => s == JobPriority.Normal)))
                .Returns(Task.FromResult(expected))
                .Verifiable();

            this.mediaService.Setup(mock => mock.GetJobAsync(
                It.Is<string>(s => s == expected.Name),
                It.Is<string>(s => s == expected.TemplateName)))
                .Returns(() =>
                {
                    expected.IsFinished = true;
                    expected.Canceled = cancelled;
                    expected.OnError = onError;
                    return Task.FromResult(expected);
                })
                .Verifiable();

            if (cleanUp)
            {
                this.mediaService.Setup(mock => mock.DeleteJobAsync(
                        It.Is<string>(s => s == expected.Name),
                        It.Is<string>(s => s == expected.TemplateName)))
                    .Returns(Task.CompletedTask)
                    .Verifiable();
                this.mediaService.Setup(mock => mock.DeleteTemplateAsync(
                        It.Is<string>(s => s == expected.TemplateName)))
                    .Returns(Task.CompletedTask)
                    .Verifiable();
                this.mediaService.Setup(mock => mock.DeleteAssetAsync(
                        It.Is<string>(s => s == inputAssetName)))
                    .Returns(Task.CompletedTask)
                    .Verifiable();
            }
            else
            {
                this.mediaService.Setup(mock => mock.DeleteJobAsync(
                        It.Is<string>(s => s == expected.Name),
                        It.Is<string>(s => s == expected.TemplateName)))
                    .Throws<Exception>();
                this.mediaService.Setup(mock => mock.DeleteTemplateAsync(
                        It.Is<string>(s => s == expected.TemplateName)))
                    .Throws<Exception>();
                this.mediaService.Setup(mock => mock.DeleteAssetAsync(
                        It.Is<string>(s => s == inputAssetName)))
                    .Throws<Exception>();
            }

            this.mediaService.Setup(mock => mock.AuthAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.mediaService.Setup(mock => mock.Dispose()).Verifiable();


            return datas;
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
                            .Input(task => task.CleanupResources, data => data.CleanUp)
                            .Input(task => task.FilePath, data => data.FilePath)
                            .Input(task => task.Preset, data => data.Presets.Single())
                            .Output(data => data.OutputAssetName, task => task.Output.Job.Outputs.First().Name)
                            .Output(data => data.OutputAssetLabel, task => task.Output.Job.Outputs.First().Label))
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
                this.CleanUp = true;
            }

            public bool CleanUp { get; set; }

            public List<string> Presets { get; set; }

            public string FilePath { get; set; }

            public string OutputAssetName {get; set; }

            public string OutputAssetLabel { get; set; }

        }
    }
}
