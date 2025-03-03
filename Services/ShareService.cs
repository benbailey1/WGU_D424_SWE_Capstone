using System.Threading.Tasks;

namespace StudentTermTracker.Services
{
    /// <summary>
    /// Service for sharing content in a MAUI Blazor Hybrid app
    /// This service wraps the MAUI Share API for use in Blazor components
    /// </summary>
    public class ShareService
    {
        /// <summary>
        /// Requests the system sharing UI to share text or a URI
        /// </summary>
        /// <param name="request">The share request details</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task RequestAsync(ShareTextRequest request)
        {
            // This wraps the MAUI Share API
            await Share.RequestAsync(request);
        }
    }

    /// <summary>
    /// Define a simple interface for registration in DI container
    /// </summary>
    public interface IShareService
    {
        Task RequestAsync(ShareTextRequest request);
    }
}