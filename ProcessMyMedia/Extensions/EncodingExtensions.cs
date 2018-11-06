namespace ProcessMyMedia.Extensions
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Azure.Management.Media.Models;

    using ProcessMyMedia.Model;

    /// <summary>
    /// Encoding Extension methods
    /// </summary>
    public static class EncodingExtensions
    {
        /// <summary>
        /// Converts to tradnsformoutputs.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IEnumerable<TransformOutput> ToTransformOutputs(this IEnumerable<EncodingOutputBase> source)
        {
            foreach(var output in source)
            {
                if(output is BuiltInPresetEncodingOutput)
                {
                    yield return ((BuiltInPresetEncodingOutput)output).ToTransformOutput();
                }
            }
        }

        /// <summary>
        /// Converts to transformoutput.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static TransformOutput ToTransformOutput(this BuiltInPresetEncodingOutput source)
        {
            return new TransformOutput(new BuiltInStandardEncoderPreset(Enum.Parse<EncoderNamedPreset>(source.Preset.ToString())), 
                onError: OnErrorType.StopProcessingJob);
        }
    }
}
