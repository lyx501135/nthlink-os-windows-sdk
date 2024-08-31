namespace nthLink.Header.Struct
{
    public struct ConfigParameter
    {
        public string ClientId; // required, SDK
        public string Language;  // required, client
        public DeviceParams Device;    // required, SDK
        public string AppVersion; // not required, client
        public string SdkVersion; // required, SDK
        public string Timezone;   // required, client
    }

    public struct DeviceParams
    {
        public string Os;     // required, SDK
        public string OsVersion;  // required, SDK
    }
}
