namespace ProcessMyMedia.Tests.Tasks.Media.Encoding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using WorkflowCore.Interface;
    using WorkflowCore.Models;

    using Moq;

    using ProcessMyMedia.Model;

    [TestClass]
    [TestCategory("Encoding")]
    public class EncodeAssetTaskUnitTest : 
        UnitTestBase<EncodeAssetTaskUnitTest.EncodeAssetWorkflow, EncodeAssetTaskUnitTest.EncodeAssetWorkflowData>
    {
        public EncodeAssetTaskUnitTest()
        {
            this.Setup();
        }

        [TestMethod]
        public void EncodeAssetWithoutPresetTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.mediaService.Setup(mock => mock.Dispose()).Verifiable();

            var datas = new EncodeAssetWorkflowData()
            {
                InputAssetName = "Asset1",
                FilePath = Directory.GetFiles(Directory.GetCurrentDirectory()).First()
            };

            this.MockIngestTask(datas.InputAssetName);

            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.IsNull(this.GetData(workflowId).OutputAssetName);
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Tasks.EncodeAssetTask.EncodingOutput)));

            this.mediaService.Verify();
        }

        [TestMethod]
        public void EncodeAssetWithoutPresetNameTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.mediaService.Setup(mock => mock.Dispose()).Verifiable();

            var datas = new EncodeAssetWorkflowData()
            {
                InputAssetName = "Asset1",
                FilePath = Directory.GetFiles(Directory.GetCurrentDirectory()).First(),
                EncodingOutput = new CustomPresetEncodingOutput()
            };

            this.MockIngestTask(datas.InputAssetName);

            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.IsNull(this.GetData(workflowId).OutputAssetName);
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Model.CustomPresetEncodingOutput.PresetName)));

            this.mediaService.Verify();
        }

        [TestMethod]
        public void EncodeAssetWithoutCodecsTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.mediaService.Setup(mock => mock.Dispose()).Verifiable();

            var datas = new EncodeAssetWorkflowData()
            {
                InputAssetName = "Asset1",
                FilePath = Directory.GetFiles(Directory.GetCurrentDirectory()).First(),
                EncodingOutput = new CustomPresetEncodingOutput()
                {
                    PresetName = "Preset1"
                }
            };

            this.MockIngestTask(datas.InputAssetName);

            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Terminated, this.GetStatus((workflowId)));
            Assert.IsNull(this.GetData(workflowId).OutputAssetName);
            Assert.AreEqual(1, this.UnhandledStepErrors.Count);
            Assert.IsInstanceOfType(this.UnhandledStepErrors[0].Exception, typeof(ArgumentException));
            Assert.IsTrue(this.UnhandledStepErrors[0].Exception.Message.Contains(nameof(ProcessMyMedia.Model.CustomPresetEncodingOutput.Codecs)));

            this.mediaService.Verify();
        }

        [TestMethod]
        public void EncodeAssetTest()
        {
            this.mediaService.Setup(mock => mock.AuthAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
            this.mediaService.Setup(mock => mock.Dispose()).Verifiable();

            var datas = new EncodeAssetWorkflowData()
            {
                InputAssetName = "Asset1",
                FilePath = Directory.GetFiles(Directory.GetCurrentDirectory()).First(),
                EncodingOutput = new CustomPresetEncodingOutput()
                {
                    PresetName = "Preset1",
                    Codecs =
                    {
                        new AacAudioCodec()
                        {
                            Bitrate = 128000,
                            Channels = 2,
                            Profile = "AACLC",
                            SamplingRate = 48000
                        }
                    }
                }
            };

            this.MockIngestTask(datas.InputAssetName);
            this.MockEncodingTask(datas);

            var workflowId = this.StartWorkflow(datas);
            WaitForWorkflowToComplete(workflowId, TimeSpan.FromSeconds(30));

            Assert.AreEqual(WorkflowStatus.Complete, this.GetStatus((workflowId)));
            Assert.IsFalse(string.IsNullOrEmpty(this.GetData(workflowId).OutputAssetName));

            this.mediaService.Verify();
        }

        private void MockIngestTask(string assetName)
        {
            this.mediaService.Setup(mock => mock.CreateOrUpdateAssetAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(() =>
                {
                    return Task.FromResult(new AssetEntity() { Name = assetName });
                })
                .Verifiable();
            this.mediaService.Setup(mock => mock.UploadFilesToAssetAsync(
                    It.Is<string>(s => s == assetName),
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<IDictionary<string, string>>()))
                .Returns(Task.CompletedTask)
                .Verifiable();
        }

        private EncodeAssetWorkflowData MockEncodingTask(EncodeAssetWorkflowData datas,
            bool cancelled = false,
            bool onError = false,
            bool cleanUp = true)
        {

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


            this.mediaService.Setup(mock => mock.StartEncodeAsync(
                    It.Is<IEnumerable<JobInputEntity>>(s => s.Count() == 1 && s.First().Name == datas.InputAssetName),
                    It.Is<IEnumerable<EncodingOutputBase>>(s => s.Count() == 1
                        && s.First() is CustomPresetEncodingOutput
                        && ((CustomPresetEncodingOutput)s.First()).PresetName.ToString() == datas.EncodingOutput.PresetName),
                    It.Is<JobPriority>(s => s == JobPriority.High)))
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
            }


            return datas;
        }


        public class EncodeAssetWorkflow : IWorkflow<EncodeAssetWorkflowData>
        {
            public string Id => nameof(EncodeAssetWorkflow);

            public int Version => 1;

            public void Build(IWorkflowBuilder<EncodeAssetWorkflowData> builder)
            {
                builder
                    .UseDefaultErrorBehavior(WorkflowErrorHandling.Terminate)
                    .StartWith<ProcessMyMedia.Tasks.IngestFileTask>()
                        .Input(task => task.AssetFilePath, data => data.FilePath)
                        .Input(task => task.AssetName, data => data.InputAssetName)
                    .Then<ProcessMyMedia.Tasks.EncodeAssetTask>()
                        .Input(task => task.CleanupResources, data => data.CleanUp)
                        .Input(task => task.Input, data => new JobInputEntity(){Name = data.InputAssetName})
                        .Input(task => task.EncodingOutput, data => data.EncodingOutput)
                        .Input(task => task.Priority, datas => JobPriority.High)
                        .Output(data => data.OutputAssetName, task => task.Output.Job.Outputs.First().Name);
            }
        }

        public class EncodeAssetWorkflowData
        {
            public EncodeAssetWorkflowData()
            {
                this.CleanUp = true;
            }

            public bool CleanUp { get; set; }

            public string InputAssetName { get; set; }

            public string FilePath { get; set; }

            public CustomPresetEncodingOutput EncodingOutput { get; set; }

            public string OutputAssetName { get; set; }

        }
    }
}
