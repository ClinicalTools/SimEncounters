using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ManualLogin : MonoBehaviour, ILogin
    {     
        [SerializeField] private TMP_InputField usernameField;
        public TMP_InputField UsernameField { get => usernameField; set => usernameField = value; }
        
        [SerializeField] private TMP_InputField passwordField;
        public TMP_InputField PasswordField { get => passwordField; set => passwordField = value; }

        [SerializeField] private Button loginButton;
        public Button LoginButton { get => loginButton; set => loginButton = value; }

        [SerializeField] private Button guestButton;
        public Button GuestButton { get => guestButton; set => guestButton = value; }

        public event LoggedInEventHandler LoggedIn;

        protected ILoadingScreen LoadingScreen { get; private set; }
        protected IPasswordLogin PasswordLogin { get; private set; }

        //[Inject]
        public void Init(ILoadingScreen loadingScreen, IPasswordLogin passwordLogin)
        {
            LoadingScreen = loadingScreen;
            PasswordLogin = passwordLogin;
        }

        public void Begin()
        {
            gameObject.SetActive(true);

            UsernameField.text = "";
            PasswordField.text = "";
            LoadingScreen.Stop();
            LoginButton.onClick.AddListener(Login);
            GuestButton.onClick.AddListener(GuestLogin);
            PasswordLogin.LoggedIn += PasswordLogin_LoggedIn;
        }

        private void GuestLogin()
        {
            LoggedIn?.Invoke(this, new LoggedInEventArgs(User.Guest));
            gameObject.SetActive(false);
        }

        private void PasswordLogin_LoggedIn(object sender, LoggedInEventArgs e)
        {
            if (e.User != null) {
                LoggedIn?.Invoke(this, e);
                gameObject.SetActive(false);
                PlayerPrefs.SetInt("StayLoggedIn", 1);
                PlayerPrefs.Save();
                return;
            }
        }

        protected virtual void Login()
        {
            PasswordLogin.Username = UsernameField.text;
            PasswordLogin.Email = UsernameField.text;
            PasswordLogin.Password = PasswordField.text;
            PasswordLogin.Begin();
        }

    }
}