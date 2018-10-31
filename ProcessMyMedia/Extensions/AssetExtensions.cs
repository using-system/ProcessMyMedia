namespace ProcessMyMedia.Extensions
{
    using System;

    using ProcessMyMedia.Model;

    /// <summary>
    /// Asset extension methods
    /// </summary>
    public static class AssetExtensions
    {
        /// <summary>
        /// To the entity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static AssetEntity ToEntity(this Microsoft.Azure.Management.Media.Models.Asset source)
        {
            if (source == null)
            {
                return null;
            }

            return new AssetEntity()
            {
                AssetID = (source.AssetId == Guid.Empty) ? string.Empty : source.AssetId.ToString(),
                Name = source.Name,
                Description = source.Description,
                CreationDate =  source.Created,
                UpdateDate =  source.LastModified
            };
        }
    }
}
