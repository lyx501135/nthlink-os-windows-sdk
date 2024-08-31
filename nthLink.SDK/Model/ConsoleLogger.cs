using nthLink.Header.Enum;
using nthLink.Header.Interface;

namespace nthLink.SDK.Model
{
    public class ConsoleLogger : ILogger
    {
        public void Log(LogLevelEnum logLevel, string message)
        {
            SetDateTime();
            SetTag(logLevel);
            WriteLine(logLevel, message, null);
        }

        public void Log(LogLevelEnum logLevel, string message, Exception e)
        {
            SetDateTime();
            SetTag(logLevel);
            WriteLine(logLevel, message, e);
        }
        private static void SetDateTime()
        {
            string dateTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            Console.Write($"[{dateTimeString}]");

#if DEBUG
            System.Diagnostics.Debug.Write($"[{dateTimeString}]");
#endif
        }
        private static void WriteLine(LogLevelEnum logLevel, string message, Exception? e)
        {
            switch (logLevel)
            {
                case LogLevelEnum.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                case LogLevelEnum.Warn:
                case LogLevelEnum.Trace:
                case LogLevelEnum.Debug:
                case LogLevelEnum.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            Console.WriteLine(message);

            if (e != null)
            {
                Console.WriteLine($"{e.Message}{Environment.NewLine}{e.StackTrace}");
            }
#if DEBUG
            System.Diagnostics.Debug.WriteLine(message);

            if (e != null)
            {
                System.Diagnostics.Debug.WriteLine($"{e.Message}{Environment.NewLine}{e.StackTrace}");
            }
#endif

            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void SetTag(LogLevelEnum logLevel)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[");

            switch (logLevel)
            {
                case LogLevelEnum.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevelEnum.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                case LogLevelEnum.Trace:
                case LogLevelEnum.Debug:
                case LogLevelEnum.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            Console.Write(logLevel.ToString());

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("]");

#if DEBUG
            System.Diagnostics.Debug.WriteLine($"[{logLevel}]");
#endif
        }
    }
}
