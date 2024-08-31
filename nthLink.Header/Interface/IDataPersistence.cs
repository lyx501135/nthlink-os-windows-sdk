namespace nthLink.Header.Interface
{
    public interface IDataPersistence
    {
        void Save(string key, string value);
        string Load(string key);
        void Cache(string key, string value);
        void DeleteCache(string key);
        void ExecuteCache();
    }
}
