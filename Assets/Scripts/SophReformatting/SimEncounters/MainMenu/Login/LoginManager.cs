namespace ClinicalTools.SimEncounters.MainMenu
{
    public class LoginManager : ILoginManager
    {
        protected ILoginManager AutoLogin { get; }
        protected ILoginManager ManualLogin { get; }

        public LoginManager(ILoginManager autoLogin, ILoginManager manualLogin)
        {
            AutoLogin = autoLogin;
            ManualLogin = manualLogin;
        }

        public WaitableResult<User> Login()
        {
            var user = new WaitableResult<User>();
            var autoLoginUser = AutoLogin.Login();
            autoLoginUser.AddOnCompletedListener((result) => AutoLoginCompleted(user, autoLoginUser));

            return user;
        }

        private void AutoLoginCompleted(WaitableResult<User> result, WaitableResult<User> autoLoginUser)
        {
            if (!autoLoginUser.IsError) {
                result.SetResult(autoLoginUser.Result, autoLoginUser.Message);
                return;
            }

            var manualLoginUser = ManualLogin.Login();
            manualLoginUser.AddOnCompletedListener((manualLoginResult) => ManualLoginCompleted(result, autoLoginUser));
        }

        private void ManualLoginCompleted(WaitableResult<User> result, WaitableResult<User> manualLoginUser)
        {
            if (manualLoginUser.IsError)
                result.SetError(manualLoginUser.Message);
            else
                result.SetResult(manualLoginUser.Result, manualLoginUser.Message);
        }
    }
}