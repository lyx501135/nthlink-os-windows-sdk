using nthLink.Header.Interface;

namespace nthLink.SDK.Model
{
    public abstract class DataPersistenceBase : IDataPersistence
    {
        private readonly Dictionary<string, string> cache = new Dictionary<string, string>();

        public void Cache(string key, string value)
        {
            if (this.cache.ContainsKey(key))
            {
                this.cache[key] = value;
            }
            else
            {
                this.cache.Add(key, value);
            }
        }

        public void DeleteCache(string key)
        {
            if (this.cache.ContainsKey(key))
            {
                this.cache.Remove(key);
            }
        }

        public void ExecuteCache()
        {
            foreach (var item in this.cache)
            {
                Save(item.Key, item.Value);
            }
        }

        public abstract string Load(string key);
        public abstract void Save(string key, string value);
    }
}
