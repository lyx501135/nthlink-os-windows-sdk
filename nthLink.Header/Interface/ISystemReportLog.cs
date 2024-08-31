using nthLink.Header.Enum;

namespace nthLink.Header.Interface
{
    /// <summary>
    /// This log will write to the local file
    /// </summary>
    public interface ISystemReportLog
    {
        Task Log(LogLevelEnum logLevel, string message);
        Task Save();
    }
}
