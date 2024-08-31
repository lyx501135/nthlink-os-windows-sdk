namespace nthLink.Header.Struct
{
    public struct FeedbackParameter
    {
        public string ClientId;    // required, SDK
        public string Language;    // not required, SDK
        public string Os;          // not required, SDK
        public string AppVersion;   // not required, SDK
        public string UtcSent;      // required, SDK
        public string FeedbackType; // required, client
        public string Description;  // not required, client
        public string ErrorCode;   // not required, client
        public string ErrorMessage; // not required, client
        public string Email;       // not required, client
    }
}
