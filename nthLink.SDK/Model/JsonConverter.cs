using nthLink.Header.Interface;

namespace nthLink.SDK.Model
{
    public class JsonConverter : IJsonConverter
    {
        public T? Deserialize<T>(string json)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default;
            }
        }

        public string Serialize(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}
