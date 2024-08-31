namespace nthLink.Header.Struct
{
    public struct ReportParameter
    {
        public string ClientId; // required, SDK
        public Events[] Events;   // required, client
    }

    public struct Events
    {
        public string Url;     // required, client
        public string UtcTime; // required, client
        public int Source;  // required, client
    }
}
