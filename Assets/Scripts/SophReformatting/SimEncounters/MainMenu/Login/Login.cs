namespace ClinicalTools.SimEncounters.MainMenu
{
    public class Login : ILogin
    {
        public event LoggedInEventHandler LoggedIn;
        protected AutoLogin AutoLogin { get; }
        protected ManualLogin ManualLogin { get; }

        public Login(AutoLogin autoLogin, ManualLogin manualLogin)
        {
            AutoLogin = autoLogin;
            ManualLogin = manualLogin;
        }

        public void Begin()
        {
            AutoLogin.LoggedIn += Auto_LoggedIn;
            AutoLogin.Begin();
        }

        private void Auto_LoggedIn(object sender, LoggedInEventArgs e)
        {
            if (e.User != null) {
                LoggedIn?.Invoke(this, e);
                return;
            }

            ManualLogin.LoggedIn += ManualLogin_LoggedIn;
            ManualLogin.Begin();
        }

        private void ManualLogin_LoggedIn(object sender, LoggedInEventArgs e)
        {
            LoggedIn?.Invoke(this, e);
        }
    }
}