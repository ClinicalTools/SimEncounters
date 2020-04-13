using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuManager : EncounterSceneManager
    {
        public static MainMenuManager MainMenuInstance => (MainMenuManager)Instance;

        [SerializeField] private ManualLogin manualLogin;
        public ManualLogin ManualLogin { get => manualLogin; set => manualLogin = value; }

        [Inject]
        protected ICategoriesReader CategoriesReader { get; set; }

        public override void Awake()
        {
            base.Awake();

            if (Instance != this)
                return;
        }

        public void Start()
        {
            var mainMenuUI = (MainMenuUI)SceneUI;
            //mainMenuUI.Login.CreateNewLogin(LoadingScreen);
            //mainMenuUI.Login.GuestLogin(LoadingScreen);
            var user = User.Guest;
            var menuEncounters = CategoriesReader.GetCategories(user);
            menuEncounters.AddOnCompletedListener(categoriesLoaded);
            var menuSceneInfo = new LoadingMenuSceneInfo(User.Guest, LoadingScreen, menuEncounters);
            mainMenuUI.Display(menuSceneInfo);
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

        private void categoriesLoaded(List<Category> categories)
        {

        }
    }
}