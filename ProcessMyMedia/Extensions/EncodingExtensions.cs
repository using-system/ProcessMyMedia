namespace ProcessMyMedia.Extensions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.Azure.Management.Media.Models;

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
        public static IEnumerable<TransformOutput> ToTransformOutputs(this IEnumerable<Model.EncodingOutputBase> source)
        {
            foreach(var output in source)
            {
                if(output is Model.BuiltInPresetEncodingOutput)
                {
                    yield return ((Model.BuiltInPresetEncodingOutput)output).ToTransformOutput();
                }

                if (output is Model.CustomPresetEncodingOutput)
                {
                    yield return ((Model.CustomPresetEncodingOutput)output).ToTransformOutput();
                }
            }
        }

        /// <summary>
        /// To the job input.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static JobInput ToJobInput(this IEnumerable<Model.JobAssetEntity> source)
        {
            return new JobInputs(source.ToJobInputs().ToList());
        }

        /// <summary>
        /// To the job inputs.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IEnumerable<JobInput> ToJobInputs(this IEnumerable<Model.JobAssetEntity> source)
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
        public static TransformOutput ToTransformOutput(this Model.BuiltInPresetEncodingOutput source)
        {
            return new TransformOutput(new BuiltInStandardEncoderPreset(Enum.Parse<EncoderNamedPreset>(source.Preset.ToString())), 
                onError: OnErrorType.StopProcessingJob);
        }

        /// <summary>
        /// To the template entity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static Model.TemplateEntity ToTemplateEntity(this Transform source)
        {
            if (source == null)
            {
                return null;
            }

            return new Model.TemplateEntity()
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
        public static TransformOutput ToTransformOutput(this Model.CustomPresetEncodingOutput source)
        {
            return new TransformOutput(new StandardEncoderPreset(
                formats: source.ToFormats().ToList(),
                codecs: source.ToCodecs().ToList(),
                filters: null),
                onError: OnErrorType.StopProcessingJob);
        }

        /// <summary>
        /// Converts to codecs.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IEnumerable<Codec> ToCodecs(this Model.CustomPresetEncodingOutput source)
        {
            foreach (var codec in source.Codecs)
            {
                if(codec is Model.H264VideoCodec)
                {
                    yield return ((Model.H264VideoCodec)codec).ToH264Video();
                }
            }
        }

        /// <summary>
        /// Converts to h264video.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static H264Video ToH264Video(this Model.H264VideoCodec source)
        {
            return new H264Video()
            {
                Layers = source.Layers.Select(layer => layer.ToH264Layer()).ToList()
            };
        }

        /// <summary>
        /// Converts to h264layer.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static H264Layer ToH264Layer(this Model.H264VideoLayer source)
        {
            return new H264Layer()
            {
                Bitrate = source.Bitrate,
                Width = source.Width,
                Height = source.Height
            };
        }

        /// <summary>
        /// Converts to formats.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IEnumerable<Format> ToFormats(this Model.CustomPresetEncodingOutput source)
        {
            foreach(var codec in source.Codecs)
            {
                if(string.IsNullOrEmpty(codec.FilenamePattern))
                {
                    continue;
                }

                if(codec is Model.H264VideoCodec)
                {
                    yield return new Mp4Format(codec.FilenamePattern);
                }
            }
        }


    }
}
