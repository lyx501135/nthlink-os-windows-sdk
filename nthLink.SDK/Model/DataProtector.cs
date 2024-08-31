using System.Security.Cryptography;
using System.Text;

namespace nthLink.SDK.Model
{
    public class DataProtector : DataPersistenceBase
    {
        private const string EXTENSION = ".dat";

        private readonly Encoding encoding;

        public DataProtector(Encoding encoding)
        {
            this.encoding = encoding;
        }

        public override string Load(string key)
        {
            string fileName = key + EXTENSION;

            if (File.Exists(fileName))
            {
                byte[] data = File.ReadAllBytes(fileName);

                byte[] content = ProtectedData.Unprotect(data, default, DataProtectionScope.LocalMachine);

                return this.encoding.GetString(content);
            }
            else
            {
                return string.Empty;
            }
        }

        public override void Save(string key, string value)
        {
            string fileName = key + EXTENSION;

            byte[] data = this.encoding.GetBytes(value);

            byte[] content = ProtectedData.Protect(data, default, DataProtectionScope.LocalMachine);

            File.WriteAllBytes(fileName, content);
        }
    }
}
