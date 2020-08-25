namespace ClinicalTools.SimEncounters.MainMenu
{
    public class LoginHandler : ILoginHandler
    {
        protected ILoginHandler AutoLogin { get; }
        protected ILoginHandler ManualLogin { get; }

        public LoginHandler(ILoginHandler autoLogin, ILoginHandler manualLogin)
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