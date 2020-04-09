using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ManualLogin : MonoBehaviour, ILoginManager
    {     
        [SerializeField] private TMP_InputField usernameField;
        public TMP_InputField UsernameField { get => usernameField; set => usernameField = value; }
        
        [SerializeField] private TMP_InputField passwordField;
        public TMP_InputField PasswordField { get => passwordField; set => passwordField = value; }

        [SerializeField] private Button loginButton;
        public Button LoginButton { get => loginButton; set => loginButton = value; }

        [SerializeField] private Button guestButton;
        public Button GuestButton { get => guestButton; set => guestButton = value; }

        [SerializeField] private MessageHandler messageHandler;
        public MessageHandler MessageHandler { get => messageHandler; set => messageHandler = value; }

        protected ILoadingScreen LoadingScreen { get; private set; }
        protected IPasswordLoginManager PasswordLoginHandler { get; private set; }

        public void Init(ILoadingScreen loadingScreen, IPasswordLoginManager passwordLoginHandler)
        {
            LoadingScreen = loadingScreen;
            PasswordLoginHandler = passwordLoginHandler;
        }

        protected virtual WaitableResult<User> CurrentWaitableResult { get; set; }
        public WaitableResult<User> Login()
        {
            if (CurrentWaitableResult == null || CurrentWaitableResult.IsCompleted)
                CurrentWaitableResult = new WaitableResult<User>();

            gameObject.SetActive(true);

            UsernameField.text = "";
            PasswordField.text = "";
            LoadingScreen.Stop();
            LoginButton.onClick.AddListener(PasswordLogin);
            GuestButton.onClick.AddListener(GuestLogin);

            return CurrentWaitableResult;
        }

        private void GuestLogin()
        {
            if (CurrentWaitableResult?.IsCompleted == false)
                CurrentWaitableResult.SetResult(User.Guest);
            gameObject.SetActive(false);
        }

        private void PasswordLogin()
        {
            var username = UsernameField.text;
            var email = UsernameField.text;
            var password = PasswordField.text;
            var user = PasswordLoginHandler.Login(username, email, password);
            user.AddOnCompletedListener((result) => ServerUserResponse(user));
        }

        protected void ServerUserResponse(WaitableResult<User> waitableResult)
        {
            if (waitableResult.IsError)
            {
                // show message maybe?
                return;
            }
            if (!CurrentWaitableResult.IsCompleted)
            {
                // show message maybe?
                return;
            }
            gameObject.SetActive(false);
            PlayerPrefs.SetInt("StayLoggedIn", 1);
            PlayerPrefs.Save();
            CurrentWaitableResult.SetResult(waitableResult.Result, waitableResult.Message);
        }
    }
}