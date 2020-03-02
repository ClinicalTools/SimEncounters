using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{

    public class DeviceIdLogin : IDeviceIdLogin
    {
        public event LoggedInEventHandler LoggedIn;
        protected IWebAddress WebAddress { get; }
        public string DeviceId { get; set; }

        public void Begin()
        {

            WWWForm form = new WWWForm();
            form.AddField("ACTION", "checkSession");
            form.AddField("deviceid", DeviceId);

            var address = WebAddress.GetUrl("Login.php");
            var webRequest = UnityWebRequest.Post(address, form);
            var requestOperation = webRequest.SendWebRequest();
            requestOperation.completed += (asyncOperation) => RequestOperation_completed(webRequest);
        }

        private void RequestOperation_completed(UnityWebRequest webRequest)
        {
            if (!string.IsNullOrWhiteSpace(webRequest.error))
                return; // throw new System.NotImplementedException();


        }
    }

    public class PasswordLogin : IPasswordLogin
    {
        public event LoggedInEventHandler LoggedIn;

        protected IWebAddress WebAddress { get; }

        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public PasswordLogin(IWebAddress webAddress)
        {
            WebAddress = webAddress;
        }


        protected virtual LoggedInEventArgs NoUsernameOrEmailArgs { get; } = new LoggedInEventArgs("No username or email provided.");
        protected virtual LoggedInEventArgs NoPasswordArgs { get; } = new LoggedInEventArgs("No password provided.");
        public void Begin()
        {
            if (string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Email))
                LoggedIn?.Invoke(this, NoUsernameOrEmailArgs);
            if (string.IsNullOrWhiteSpace(Password))
                LoggedIn?.Invoke(this, NoPasswordArgs);
        }
    }

    public class AutoLogin : ILogin
    {
        public event LoggedInEventHandler LoggedIn;

        public void Begin()
        {

        }
    }
    public class ManualLogin : MonoBehaviour, ILogin
    {
        [SerializeField] private Button loginButton;
        public Button LoginButton { get => loginButton; set => loginButton = value; }

        public event LoggedInEventHandler LoggedIn;
        public ILoadingScreen LoadingScreen { get; }

        public ManualLogin(ILoadingScreen loadingScreen)
        {
            LoadingScreen = loadingScreen;
        }

        public void Begin()
        {
            LoadingScreen.Stop();
            LoginButton.onClick.AddListener(Login);
        }

        protected virtual void Login()
        {

        }

    }

    public interface IDeviceIdLogin : ILogin
    {
        string DeviceId { get; set; }
    }
    public interface IPasswordLogin : ILogin
    {
        string Email { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
    public interface ILogin
    {
        event LoggedInEventHandler LoggedIn;

        void Begin();
    }
    public delegate void LoggedInEventHandler(object sender, LoggedInEventArgs e);
    public class LoggedInEventArgs
    {
        public User User { get; }
        public string Message { get; }
        public LoggedInEventArgs(string message)
        {
            Message = message;
        }
        public LoggedInEventArgs(User user, string message)
        {
            User = user;
            Message = message;
        }
    }
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
            if (e.User != null)
            {
                LoggedIn?.Invoke(this, e);
                return;
            }

            ManualLogin.LoggedIn += ManualLogin_LoggedIn1;
        }

        private void ManualLogin_LoggedIn1(object sender, LoggedInEventArgs e)
        {
            LoggedIn?.Invoke(this, e);
        }
    }
}