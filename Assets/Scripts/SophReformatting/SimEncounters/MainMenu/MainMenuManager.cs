using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuManager : EncounterSceneManager
    {
        public static MainMenuManager MainMenuInstance => (MainMenuManager)Instance;

        [SerializeField] private ManualLogin manualLogin;

        public override void Awake()
        {
            base.Awake();

            if (Instance != this)
                return;

            StartCoroutine(StartScene());
        }

        public IEnumerator StartScene()
        {
            var webAddress = new WebAddress();
            var userParser = new UserParser();
            var deviceIdLogin = new DeviceIdLogin(webAddress, userParser);
            var autoLogin = new AutoLogin(deviceIdLogin);
            var passwordLogin = new PasswordLogin(webAddress, userParser);
            manualLogin.Init(LoadingScreen.Instance, passwordLogin);

            var login = new Login(autoLogin, manualLogin);
            login.LoggedIn += LoggedIn;
            login.Begin();

            yield return null;
        }

        private void LoggedIn(object sender, LoggedInEventArgs e)
        {
            Debug.LogError(e.User);
            new MainMenuScene(e.User, (MainMenuUI)SceneUI);
        }
    }
}