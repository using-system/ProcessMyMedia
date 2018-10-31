namespace ProcessMyMedia.Model
{
    using System.Collections.Generic;

    public class JobEntity
    {
        public string JobID { get; set; }
        public IEnumerable<string> InputAssetNames { get; set; }

        public IEnumerable<string> OutputAssetNames { get; set; }
    }
}
