using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace nthLink.SDK
{
    public static class Util
    {
        public static string GetWorkDirectory()
        {
            return Environment.CurrentDirectory;
        }

        public static Encoding GetDefaultEncoding() => Encoding.UTF8;

        public static class Net
        {
            public static async Task<int> TCPingAsync(IPAddress ip, int port, int timeout = 1000, CancellationToken ct = default)
            {
                using var client = new TcpClient(ip.AddressFamily);

                var stopwatch = Stopwatch.StartNew();

                var task = client.ConnectAsync(ip, port);

                var resTask = await Task.WhenAny(task, Task.Delay(timeout, ct));

                stopwatch.Stop();
                if (resTask == task && client.Connected)
                {
                    var t = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                    return t;
                }

                return timeout;
            }

            public static async Task<int> ICMPingAsync(IPAddress ip, int timeout = 1000)
            {
                var reply = await new Ping().SendPingAsync(ip, timeout);

                if (reply.Status == IPStatus.Success)
                    return Convert.ToInt32(reply.RoundtripTime);

                return timeout;
            }
        }

        internal static Version GetFileVersion(string driver)
        {
            if (File.Exists(driver) &&
                FileVersionInfo.GetVersionInfo(driver).FileVersion is string fileVersion &&
                Version.TryParse(fileVersion, out Version? version) &&
                version != null)
            {
                return version;
            }
            else
            {
                return new Version(0, 0, 0, 0);
            }
        }
        public static int FreeTcpPort()
        {
            try
            {
                TcpListener l = new TcpListener(IPAddress.Loopback, 0);
                l.Start();
                int port = ((IPEndPoint)l.LocalEndpoint).Port;
                l.Stop();
                return port;
            }
            catch
            {

            }
            return -1;
        }
    }
}
