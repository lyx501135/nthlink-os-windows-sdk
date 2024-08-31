using nthLink.Header.Enum;

namespace nthLink.Header.Struct
{
    public class AccountEventArgs : EventArgs
    {
        public AccountActionEnum AccountAction { get; }
        public bool IsSuccess { get; }
        public string Account { get; }
        public AccountEventArgs(bool isSuccess, string account, AccountActionEnum accountAction)
        {
            IsSuccess = isSuccess;
            Account = account;
            AccountAction = accountAction;
        }
    }
}
