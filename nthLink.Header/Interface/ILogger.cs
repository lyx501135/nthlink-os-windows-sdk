using nthLink.Header.Enum;

namespace nthLink.Header.Interface
{
    public interface ILogger
    {
        void Log(LogLevelEnum logLevel, string message);
        void Log(LogLevelEnum logLevel, string message, Exception e);
    }
}
