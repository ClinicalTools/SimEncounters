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
            autoLoginUser.AddOnCompletedListener((result) => AutoLoginCompleted(user, result));

            return user;
        }

        private void AutoLoginCompleted(WaitableResult<User> result, WaitedResult<User> autoLoginUser)
        {
            if (!autoLoginUser.IsError()) {
                result.SetResult(autoLoginUser.Value);
                return;
            }

            var manualLoginUser = ManualLogin.Login();
            manualLoginUser.AddOnCompletedListener((manualLoginResult) => ManualLoginCompleted(result, manualLoginResult));
        }

        private void ManualLoginCompleted(WaitableResult<User> result, WaitedResult<User> manualLoginUser)
        {
            if (manualLoginUser.IsError())
                result.SetError(manualLoginUser.Exception);
            else
                result.SetResult(manualLoginUser.Value);
        }
    }
}