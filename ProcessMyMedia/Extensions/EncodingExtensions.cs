namespace ProcessMyMedia.Extensions
{
    using System;
    using System.Linq;
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

                if (output is CustomPresetEncodingOutput)
                {
                    yield return ((CustomPresetEncodingOutput)output).ToTransformOutput();
                }
            }
        }

        /// <summary>
        /// To the job input.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static JobInput ToJobInput(this IEnumerable<JobAssetEntity> source)
        {
            return new JobInputs(source.ToJobInputs().ToList());
        }

        /// <summary>
        /// To the job inputs.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IEnumerable<JobInput> ToJobInputs(this IEnumerable<JobAssetEntity> source)
        {
            foreach (var input in source)
            {
                yield return new JobInputAsset(input.Name, label: input.Label);
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

        /// <summary>
        /// To the template entity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static TemplateEntity ToTemplateEntity(this Transform source)
        {
            if (source == null)
            {
                return null;
            }

            return new TemplateEntity()
            {
                Name = source.Name,
                Created = source.Created
            };
        }

        /// <summary>
        /// To the transform output.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static TransformOutput ToTransformOutput(this CustomPresetEncodingOutput source)
        {
            return new TransformOutput(new StandardEncoderPreset(),
                onError: OnErrorType.StopProcessingJob);
        }
    }
}
