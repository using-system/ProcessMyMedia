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
                case Type fileSystemType when typeof(Model.FileSystemDataPath).IsAssignableFrom(fileSystemType):
                    return ((Model.FileSystemDataPath)source).GetPathProperties();
                case Type genericType when genericType == typeof(Model.GenericDataPath):
                    return ((Model.GenericDataPath)source).GetPathProperties();
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
        public static IEnumerable<JProperty> GetActivityProperties(this Model.DataPath source)
        {
            if (source == null)
            {
                return null;
            }

            switch (source.GetType())
            {
                case Type fileSystemType when typeof(Model.FileSystemDataPath).IsAssignableFrom(fileSystemType):
                    return ((Model.FileSystemDataPath)source).GetActivityProperties();
                case Type genericType when genericType == typeof(Model.GenericDataPath):
                    return ((Model.GenericDataPath)source).GetActivityProperties();
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

            if (!string.IsNullOrEmpty(source.FileName))
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
        /// Gets the path properties.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static JObject GetPathProperties(this Model.GenericDataPath source)
        {
            if(source == null
               || source.PathProperties == null)
            {
                return null;
            }

            return JObject.FromObject(source.PathProperties);
        }

        /// <summary>
        /// Gets the copy properties.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IEnumerable<JProperty> GetActivityProperties(this Model.FileSystemDataPath source)
        {
            if (source.Recursive.HasValue)
            {
                yield return new JProperty("recursive", source.Recursive.Value);
            }

            if (source.PreserveHierarchy.HasValue)
            {
                if (source.PreserveHierarchy.Value)
                {
                    yield return new JProperty("copyBehavior", "PreserveHierarchy");
                }
                else
                {
                    yield return new JProperty("copyBehavior", "FlattenHierarchy");
                }
            }
        }

        /// <summary>
        /// Gets the activity properties.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static IEnumerable<JProperty> GetActivityProperties(this Model.GenericDataPath source)
        {
            if(source == null
               || source.ActivityProperties == null)
            {
                return null;
            }

            return JObject.FromObject(source.ActivityProperties).Properties();
        }
    }
}
