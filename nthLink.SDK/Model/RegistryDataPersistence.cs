using Microsoft.Win32;
using nthLink.Header;

namespace nthLink.SDK.Model
{
    public class RegistryDataPersistence : DataPersistenceBase
    {
        private const string RegistryPath = $"SOFTWARE\\{Const.String.ProductName}\\Data";

        public override string Load(string key)
        {
            RegistryKey? registryKey = Registry.LocalMachine
                .OpenSubKey(RegistryPath);

            if (registryKey == null)
            {
                registryKey = Registry.LocalMachine
                .CreateSubKey(RegistryPath);

                if (registryKey == null)
                {
                    return string.Empty;
                }
                else
                {
                    return Load(key);
                }
            }

            if (registryKey.GetValue(key) is string str)
            {
                return str;
            }
            else
            {
                return string.Empty;
            }
        }

        public override void Save(string key, string value)
        {
            RegistryKey? registryKey = Registry.LocalMachine
                .OpenSubKey(RegistryPath, true);

            if (registryKey == null)
            {
                registryKey = Registry.LocalMachine.CreateSubKey(RegistryPath);
            }

            registryKey.SetValue(key, value);
        }
    }
}
