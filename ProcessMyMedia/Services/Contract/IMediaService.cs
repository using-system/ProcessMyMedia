namespace ProcessMyMedia.Services.Contract
{
    using System.Collections.Generic;

    using ProcessMyMedia.Model;

    using System.Threading.Tasks;

    /// <summary>
    /// media service contract
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Services.Contract.IService" />
    public interface IMediaService : IService
    {
        /// <summary>
        /// Authenticate.
        /// </summary>
        /// <returns></returns>
        Task AuthAsync();

        /// <summary>
        /// Creates the or update asset.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <param name="assetDescription">The asset description.</param>
        /// <param name="storageAccountName">Name of the storage account.</param>
        /// <returns></returns>
        Task<AssetEntity> CreateOrUpdateAssetAsync(string assetName, string assetDescription = "", string storageAccountName ="");

        /// <summary>
        /// Uploads the files to asset.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <param name="files">The files.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns></returns>
        Task UploadFilesToAssetAsync(string assetName,
            IEnumerable<string> files,
            IDictionary<string, string> metadata = null);

        /// <summary>
        /// Downloads the files.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <param name="directoryToDownload">The directory to download.</param>
        /// <returns></returns>
        Task DownloadFilesAsync(string assetName, string directoryToDownload);

        /// <summary>
        /// Deletes the asset.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <returns></returns>
        Task DeleteAssetAsync(string assetName);

        /// <summary>
        /// Annalyses the specified asset name.
        /// </summary>
        /// <param name="assetName">Name of the asset.</param>
        /// <returns></returns>
        Task<JobEntity> StartAnalyseAsync(string assetName);

        /// <summary>
        /// Deletes the job.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="transformationName">Name of the transformation.</param>
        /// <returns></returns>
        Task DeleteJobAsync(string jobName, string transformationName);
    }
}
