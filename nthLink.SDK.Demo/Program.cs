using nthLink.Header.Interface;
using nthLink.SDK.Extension;

namespace nthLink.SDK.Demo
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            IContainerProvider containerProvider = nthLink.SDK.Entry.CreateContainerRegistry("MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEA9/W/2SBYDG9rlQ3XJt39p2ebGsZo81o1Oq6cPwP0BHIvfjeWf3l0fQaNS1zAgTenyBWNxV4Sk526mGFnnpeP2Fjx6YMsIdULSFoz63is1Inii82DGLE5CWvzM1RvZkV8rQ5UcWRPh3je2g6Vzyd0AKA0xxTqvQQbnsK1sEK9biMI2242yvzUEOI36M9dVr5WOzZurIC+RgE4OjAsfGNc5rNu2ILO+T0Zq5YOiOaqh1CmvlVwlazvjUcdsEPitsMi01w4DLdAi8qJFO1dNNaEjDFMVXT5Sxk/lmpoeRzG+aYBnd3LlIDlaaSG1ja0gxf8GHoqckLAiiV8OyDJA5JnySGh0rjkuUkncmhAyrK6bEFnQYhaqxXEEUTikKhYFi0A/17JOkRXyOW/uNhS3lQoZ42GkYlAaKSqFR4TA6nNmpup3eTyGpUKwjZqy37PT8SKytD9I1yM3No5KvtSV/lh05yf0+JJZL0a4ChDLWa0OEuuaY/ocKO4VuVB+3KpbgfF8uAOvGBMk60QUGoG6vDKjm2TIzxYCWojihmThx319mFytovJd/JP/c8vXVvDO4fJOYMbPhjYMju8/HmH2atEW1dgnzDHpO9ngALzJ8XM94V0DGPvqqKg/UqOCCYZy9Zc4YofE34/7tIicI/ho4KwzMZ1ek4b30+kpMJ/b0xQ0UkCAwEAAQ==")
                .LoadModule()
                .InitializeModuleAndCreateContainerProvider();
            Application.Run(new Form1(containerProvider));
        }
    }
}