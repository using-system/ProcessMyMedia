namespace ProcessMyMedia.Extensions
{
    using Microsoft.Azure.Management.DataFactory.Models;

    /// <summary>
    /// Linked Service Extension methids
    /// </summary>
    public static class LinkedServiceExtensions
    {
        /// <summary>
        /// To the entity.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static  Model.LinkedServiceEntity ToEntity(this LinkedServiceResource source)
        {
            if (source == null)
            {
                return null;
            }

            return new Model.LinkedServiceEntity()
            {
                Name = source.Name,
                Type = source.Type,
                Description = source.Properties.Description
            };
        }
    }
}
