namespace ProcessMyMedia.Services
{
    using System;

    /// <summary>
    /// Delay Service
    /// </summary>
    /// <seealso cref="ProcessMyMedia.Services.Contract.IDelayService" />
    public class DelayService : Contract.IDelayService
    {

        /// <summary>
        /// Gets the time to sleep.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <returns></returns>
        public TimeSpan GetTimeToSleep(DateTime startDate)
        {
            double totalSeconds = (DateTime.Now - startDate.ToLocalTime()).TotalSeconds;

            if (totalSeconds < 60)
            {
                return TimeSpan.FromSeconds(5);
            }
            else if (totalSeconds < 60 * 5)
            {
                return TimeSpan.FromSeconds(30);
            }
            else if (totalSeconds < 60 * 30)
            {
                return TimeSpan.FromSeconds(60);
            }
            else
            {
                return TimeSpan.FromMinutes(3);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {

        }
    }
}
