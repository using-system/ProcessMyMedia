namespace ProcessMyMedia.Tests.Tasks.Media.Encoding
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ProcessMyMedia.Model;

    [TestClass]
    [TestCategory("Encoding")]
    public class EncodeAssetTaskUnitTest
    {
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
