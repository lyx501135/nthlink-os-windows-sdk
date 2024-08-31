using nthLink.Header.Enum;
using nthLink.Header.Interface;
using System.Runtime.InteropServices;

namespace nthLink.SDK.Kernel;

public class Redirector : IRedirector
{
    private readonly ILogger logger;
    public Redirector(ILogger logger)
    {
        this.logger = logger;
    }
    public Task<bool> RegisterAsync(string path)
    {
        return Task.Run(() => aio_register(path));
    }
    public Task<bool> UnregisterAsync(string path)
    {
        return Task.Run(() => aio_unregister(path));
    }
    public bool Dial(RedirectorNameEnum name, bool value)
    {
        this.logger.Log(LogLevelEnum.Info, $"[Redirector] Dial {name}: {value}");
        return aio_dial((int)name, value.ToString().ToLower());
    }
    public bool Dial(RedirectorNameEnum name, string value)
    {
        this.logger.Log(LogLevelEnum.Info, $"[Redirector] Dial {name}: {value}");
        return aio_dial((int)name, value);
    }
    public Task<bool> InitAsync()
    {
        return Task.Run(aio_init);
    }
    public Task<bool> FreeAsync()
    {
        return Task.Run(aio_free);
    }

    private const string Redirector_bin = $"Redirector.dll";

    [DllImport(Redirector_bin, CallingConvention = CallingConvention.Cdecl)]
    private static extern bool aio_register([MarshalAs(UnmanagedType.LPWStr)] string value);

    [DllImport(Redirector_bin, CallingConvention = CallingConvention.Cdecl)]
    private static extern bool aio_unregister([MarshalAs(UnmanagedType.LPWStr)] string value);

    [DllImport(Redirector_bin, CallingConvention = CallingConvention.Cdecl)]
    private static extern bool aio_dial(int name, [MarshalAs(UnmanagedType.LPWStr)] string value);

    [DllImport(Redirector_bin, CallingConvention = CallingConvention.Cdecl)]
    private static extern bool aio_init();

    [DllImport(Redirector_bin, CallingConvention = CallingConvention.Cdecl)]
    private static extern bool aio_free();

    [DllImport(Redirector_bin, CallingConvention = CallingConvention.Cdecl)]
    private static extern ulong aio_getUP();

    [DllImport(Redirector_bin, CallingConvention = CallingConvention.Cdecl)]
    private static extern ulong aio_getDL();
}