using nthLink.Header.Enum;

namespace nthLink.Header.Struct
{
    public struct VpnServiceFunctionArgs
    {
        public FunctionEnum Function { get; }

        public VpnServiceFunctionArgs(FunctionEnum function)
        {
            Function = function;
        }
    }
}
