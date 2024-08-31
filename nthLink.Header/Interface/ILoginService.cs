using nthLink.Header.Struct;
using System.ComponentModel;

namespace nthLink.Header.Interface
{
    public interface ILoginService : INotifyPropertyChanged
    {
        public event EventHandler<AccountEventArgs>? AccountStateChanged;
        bool IsLogined { get; }
        string CurrentAccount { get; }
        int VerificationCodeCount { get; }
        Task Login(string account, string password);
        Task Logout();
        Task ResetPassword(string account);
        Task VerificationResetPasswordCode(string account, string code, string password);
        Task ResendResetPasswordCode(string account);
        Task ForgetPassword(string account);
        Task VerificationForgetPasswordCode(string account, string code, string password);
        Task ResendForgetPasswordCode(string account);


    }
}
