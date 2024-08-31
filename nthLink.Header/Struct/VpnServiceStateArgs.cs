using nthLink.Header.Enum;

namespace nthLink.Header.Struct
{
    public struct VpnServiceStateArgs
    {
        public StateEnum State { get; }
        public string Message { get; }
        public VpnServiceStateArgs(StateEnum state, string message)
        {
            State = state;
            Message = message;
        }
    }
}
