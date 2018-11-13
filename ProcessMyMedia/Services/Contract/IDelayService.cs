namespace ProcessMyMedia.Services.Contract
{
    using System;

    /// <summary>
    /// Delay Service Contract
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Services.Contract.IService" />
    public interface IDelayService : IService
    {
        TimeSpan GetTimeToSleep(DateTime startDate);
    }
}
