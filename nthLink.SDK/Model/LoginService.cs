using nthLink.Header.Enum;
using nthLink.Header.Interface;
using nthLink.Header.Struct;

namespace nthLink.SDK.Model
{
    class LoginService : NotifyPropertyChangedBase, ILoginService
    {
        private bool isLogined;

        public bool IsLogined
        {
            get { return this.isLogined; }
            set { SetProperty(ref this.isLogined, value); }
        }

        public event EventHandler<AccountEventArgs>? AccountStateChanged;

        public string CurrentAccount { get; private set; } = string.Empty;

        public int VerificationCodeCount { get; } = 5;
        #region Login
        public async Task Login(string account, string password)
        {
            await Task.Delay(500);

            IsLogined = true;

            CurrentAccount = account;

            if (AccountStateChanged != null)
            {
                AccountStateChanged.Invoke(this, new AccountEventArgs(IsLogined, account, AccountActionEnum.Login));
            }
        }

        public async Task Logout()
        {
            if (!string.IsNullOrEmpty(CurrentAccount))
            {
                await Task.Delay(500);

                if (AccountStateChanged != null)
                {
                    AccountStateChanged.Invoke(this, new AccountEventArgs(IsLogined, CurrentAccount, AccountActionEnum.Logout));
                }

                CurrentAccount = string.Empty;

                IsLogined = false;
            }
        }
        #endregion Login
        #region ResetPassword
        public async Task ResetPassword(string account)
        {
            bool ok;

            await Task.Delay(500);

            ok = true;

            if (AccountStateChanged != null)
            {
                AccountStateChanged.Invoke(this, new AccountEventArgs(ok, account, AccountActionEnum.ResetPassword));
            }
        }

        public async Task VerificationResetPasswordCode(string account, string code, string password)
        {
            bool ok;

            await Task.Delay(500);

            ok = true;

            if (AccountStateChanged != null)
            {
                AccountStateChanged.Invoke(this, new AccountEventArgs(ok, account, AccountActionEnum.VerificationResetPasswordCode));
            }
        }

        public async Task ResendResetPasswordCode(string account)
        {
            bool ok;

            await Task.Delay(500);

            ok = true;

            if (AccountStateChanged != null)
            {
                AccountStateChanged.Invoke(this, new AccountEventArgs(ok, account, AccountActionEnum.ResendVerificationResetPasswordCode));
            }
        }
        #endregion ResetPassword
        #region ForgetPassword
        public async Task ForgetPassword(string account)
        {
            bool ok;

            await Task.Delay(500);

            ok = true;

            if (AccountStateChanged != null)
            {
                AccountStateChanged.Invoke(this, new AccountEventArgs(ok, account, AccountActionEnum.ForgetPassword));
            }
        }

        public async Task VerificationForgetPasswordCode(string account, string code, string password)
        {
            bool ok;

            await Task.Delay(500);

            ok = true;

            if (AccountStateChanged != null)
            {
                AccountStateChanged.Invoke(this, new AccountEventArgs(ok, account, AccountActionEnum.VerificationForgetPasswordCode));
            }
        }

        public async Task ResendForgetPasswordCode(string account)
        {
            bool ok;

            await Task.Delay(500);

            ok = true;

            if (AccountStateChanged != null)
            {
                AccountStateChanged.Invoke(this, new AccountEventArgs(ok, account, AccountActionEnum.ResendVerificationForgetPasswordCode));
            }
        }
        #endregion ForgetPassword
    }
}
