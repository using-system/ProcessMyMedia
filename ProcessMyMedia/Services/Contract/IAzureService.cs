namespace ProcessMyMedia.Services.Contract
{
    using System.Threading.Tasks;

    public interface IAzureService : IService
    {
        /// <summary>
        /// Authenticate.
        /// </summary>
        /// <returns></returns>
        Task AuthAsync();
    }
}
