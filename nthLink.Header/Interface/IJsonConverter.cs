namespace nthLink.Header.Interface
{
    public interface IJsonConverter
    {
        string Serialize(object obj);
        T? Deserialize<T>(string json);
    }
}
