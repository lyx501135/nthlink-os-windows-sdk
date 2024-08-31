namespace nthLink.Header.Struct
{
    public struct ProxyMessage<T>
    {
        public string ProxyGuid;
        public string Message;
        public T Args;
    }
}
