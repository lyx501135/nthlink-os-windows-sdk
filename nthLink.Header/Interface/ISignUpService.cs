using nthLink.Header.Struct;

namespace nthLink.Header.Interface
{
    public interface ISignUpService
    {
        event EventHandler<AccountEventArgs> AccountStateChanged;
        int VerificationCodeCount { get; }
        Task SignUp(string account, string password);
        Task VerificationSignUpCode(string code);
        Task ResendVerificationCode();
    }
}
