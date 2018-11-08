namespace ProcessMyMedia.Model
{
    public class CustomPresetEncodingOutput : EncodingOutputBase
    {
        public string PresetName { get; set; }

        public override string Label => this.PresetName;
    }
}
