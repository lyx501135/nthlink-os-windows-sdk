namespace nthLink.SDK.Model
{
    internal class FileDataPersistence : DataPersistenceBase
    {
        public override string Load(string key)
        {
            string path = Path.Combine(Environment.CurrentDirectory, key);

            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            else
            {
                return string.Empty;
            }
        }

        public override void Save(string key, string value)
        {
            string path = Path.Combine(Environment.CurrentDirectory, key);

            File.WriteAllText(path, value); 
        }
    }
}
