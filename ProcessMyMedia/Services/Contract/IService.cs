namespace ProcessMyMedia.Services.Contract
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Service contract
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IService : IDisposable
    {
        /// <summary>
        /// Authenticate.
        /// </summary>
        /// <returns></returns>
        Task AuthAsync();
    }
}
