using nthLink.Header.Enum;
using nthLink.Header.Interface;
using nthLink.Header.Struct;

namespace nthLink.SDK.Model
{
    public class SignUpService : ISignUpService
    {
        public int VerificationCodeCount { get; } = 5;

        public event EventHandler<AccountEventArgs>? AccountStateChanged;

        private string? currentAccount;

        public async Task SignUp(string account, string password)
        {
            await Task.Delay(500);

            this.currentAccount = account;

            if (AccountStateChanged != null)
            {
                AccountStateChanged.Invoke(this, new AccountEventArgs(true, this.currentAccount, AccountActionEnum.SignUp));
            }
        }

        public async Task VerificationSignUpCode(string code)
        {
            bool ok;
            string account = string.Empty;
            if (string.IsNullOrEmpty(this.currentAccount))
            {
                ok = false;
            }
            else
            {
                await Task.Delay(500);

                ok = true;

                account = this.currentAccount;
            }

            if (AccountStateChanged != null)
            {
                AccountStateChanged.Invoke(this, new AccountEventArgs(ok, account, AccountActionEnum.VerificationSignUpCode));
            }
        }

        public async Task ResendVerificationCode()
        {
            bool ok;
            string account = string.Empty;
            if (string.IsNullOrEmpty(this.currentAccount))
            {
                ok = false;
            }
            else
            {
                await Task.Delay(500);

                ok = true;

                account = this.currentAccount;
            }

            if (AccountStateChanged != null)
            {
                AccountStateChanged.Invoke(this, new AccountEventArgs(ok, account, AccountActionEnum.ResendVerificationSignUpCode));
            }
        }
    }
}
