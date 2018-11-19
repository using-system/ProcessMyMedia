namespace ProcessMyMedia.Extensions
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Data path extension methods
    /// </summary>
    public static class DataPathExtensions
    {
        /// <summary>
        /// Gets the path properties.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static JObject GetPathProperties(this Model.DataPath source)
        {
            if (source == null)
            {
                return null;
            }

            switch (source.GetType())
            {
                case Type fileSystemType when fileSystemType == typeof(Model.FileSystemDataPath):
                    return ((Model.FileSystemDataPath) source).GetPathProperties();
                default:
                    throw new NotImplementedException($"{source.GetType()} is not supported");
            }
        }


        /// <summary>
        /// Gets the copy properties.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IEnumerable<JProperty> GetCopyProperties(this Model.DataPath source)
        {
            if (source == null)
            {
                return null;
            }

            switch (source.GetType())
            {
                case Type fileSystemType when fileSystemType == typeof(Model.FileSystemDataPath):
                    return ((Model.FileSystemDataPath)source).GetCopyProperties();
                default:
                    throw new NotImplementedException($"{source.GetType()} is not supported");
            }
        }

        /// <summary>
        /// Gets the path properties.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static JObject GetPathProperties(this Model.FileSystemDataPath source)
        {
            if (source == null)
            {
                return null;
            }

            JObject properties = new JObject();

            if(!string.IsNullOrEmpty(source.FileName))
            {
                properties.Add("fileName", source.FileName);
            }

            if (!string.IsNullOrEmpty(source.FolderPath))
            {
                properties.Add("folderPath", source.FolderPath);
            }

            return properties;
        }

        /// <summary>
        /// Gets the copy properties.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IEnumerable<JProperty> GetCopyProperties(this Model.FileSystemDataPath source)
        {
            if (source.Recursive.HasValue)
            {
                yield return new JProperty("recursive", source.Recursive.Value);
            }
        }
    }
}
