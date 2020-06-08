using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class LoginHandler : MonoBehaviour
    {
        public ManualLogin ManualLoginPrefab { get => manualLoginPrefab; set => manualLoginPrefab = value; }
        [SerializeField] private ManualLogin manualLoginPrefab;

        public Transform LoginParent { get => loginParent; set => loginParent = value; }
        [SerializeField] private Transform loginParent;

        public event Action<User> LoggedIn;



        public void Logout(ILoadingScreen loadingScreen)
        {
            /*
            var userParser = new UserParser();
            var passwordLogin = new PasswordLogin(new WebAddressBuilder(), userParser);
            var manualLogin = Instantiate(ManualLoginPrefab, LoginParent);
            manualLogin.Init(loadingScreen, passwordLogin);
            manualLogin.LoggedIn += (sender, e) => UserLoggedIn(loadingScreen, e);
            manualLogin.Begin();*/
        }

        public void CreateNewLogin(ILoadingScreen loadingScreen)
        {

            /*
            var userParser = new UserParser();
            var deviceIdLogin = new DeviceIdLogin(new WebAddressBuilder(), userParser);
            var autoLogin = new AutoLogin(deviceIdLogin);
            var passwordLogin = new PasswordLogin(new WebAddressBuilder(), userParser);
            var manualLogin = Instantiate(ManualLoginPrefab, LoginParent);
            manualLogin.Init(loadingScreen, passwordLogin);
            var login = new Login(autoLogin, manualLogin);
            login.LoggedIn += (sender, e) => UserLoggedIn(loadingScreen, e);
            login.Begin();*/
        }
        /*
        private void UserLoggedIn(ILoadingScreen loadingScreen, LoggedInEventArgs e)
        {
            var x = new MainMenuSceneStarter(new MobileScenePathData());
            //var info = new InfoNeededForMainMenuToHappen(e.User, loadingScreen, new EncountersInfoReader());

            //x.StartMainMenu(EncounterSceneManager.EncounterInstance, info);
            LoggedIn?.Invoke(e.User);
        }*/
    }
}