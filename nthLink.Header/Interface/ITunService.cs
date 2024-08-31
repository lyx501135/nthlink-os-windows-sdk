using nthLink.Header.Struct;

namespace nthLink.Header.Interface
{
    public interface ITunService
    {
        Task StopAsync();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="mode"></param>
        /// <returns>true is success</returns>
        Task<bool> StartAsync(ServerSettingBase server);
    }
}
