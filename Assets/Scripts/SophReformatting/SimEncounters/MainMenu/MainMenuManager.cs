using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuManager : EncounterSceneManager
    {
        public static MainMenuManager MainMenuInstance => (MainMenuManager)Instance;

        [SerializeField] private ManualLogin manualLogin;
        public ManualLogin ManualLogin { get => manualLogin; set => manualLogin = value; }

        public override void Awake()
        {
            base.Awake();

            if (Instance != this)
                return;
        }

        public void Start()
        {
            var mainMenuUI = (MainMenuUI)SceneUI;
            mainMenuUI.Login.CreateNewLogin();
            /*
            var webAddress = new WebAddress();
            var userParser = new UserParser();
            var deviceIdLogin = new DeviceIdLogin(webAddress, userParser);
            var autoLogin = new AutoLogin(deviceIdLogin);
            var passwordLogin = new PasswordLogin(webAddress, userParser);
            manualLogin.Init(LoadingScreen.Instance, passwordLogin);

            var login = new Login(autoLogin, manualLogin);
            login.LoggedIn += LoggedIn;
            login.Begin();*/
        }

        private void LoggedIn(object sender, LoggedInEventArgs e)
        {
            MainMenuSceneLoader.StartMainMenu(this, e.User);
        }
    }
}