using ImaginationOverflow.UniversalDeepLinking;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public interface ILogoutHandler
    {
        event Action Logout;
    }

    public class MainMenuManager : SceneManager, IMenuSceneDrawer
    {
        public BaseMenuSceneDrawer MenuDrawer { get => menuDrawer; set => menuDrawer = value; }
        [SerializeField] private BaseMenuSceneDrawer menuDrawer;
        public LoadingScreen LoadingScreen { get => loadingScreen; set => loadingScreen = value; }
        [SerializeField] private LoadingScreen loadingScreen;
        public SomethingSomethingLogin LoginHandler { get => login; set => login = value; }
        [SerializeField] private SomethingSomethingLogin login;
        public GameObject WelcomeScreen { get => welcomeScreen; set => welcomeScreen = value; }
        [SerializeField] private GameObject welcomeScreen;

        protected IMenuEncountersInfoReader MenuInfoReader { get; set; }
        protected IEncounterQuickStarter EncounterQuickStarter { get; set; }
        [Inject]
        public virtual void Inject(IMenuEncountersInfoReader menuInfoReader, IEncounterQuickStarter encounterQuickStarter)
        {
            MenuInfoReader = menuInfoReader;
            EncounterQuickStarter = encounterQuickStarter;
        }

        protected override void Awake()
        {
            base.Awake();

            if (MenuDrawer is ILogoutHandler logoutHandler)
                logoutHandler.Logout += Logout;
        }

        protected override void StartAsInitialScene()
        {
            if (LoginHandler != null)
                ShowInitialLogin();
            else
                Login(User.Guest);
        }

        protected override void StartAsLaterScene()
        {
            if (WelcomeScreen != null)
                WelcomeScreen.SetActive(false);

            Destroy(LoadingScreen.gameObject);
        }

        protected LoadingMenuSceneInfo SceneInfo { get; set; }
        public void Display(LoadingMenuSceneInfo sceneInfo)
        {
            SceneInfo = sceneInfo;
            DeepLinkManager.Instance.LinkActivated += Instance_LinkActivated;
            MenuDrawer.Display(sceneInfo);
        }
        protected virtual void Logout()
        {
            var user = LoginHandler.Login();
            user.AddOnCompletedListener(Login);
        }
        protected virtual void ShowInitialLogin()
        {
            var user = LoginHandler.InitialLogin(LoadingScreen);
            user.AddOnCompletedListener(Login);
        }

        protected virtual void Login(WaitedResult<User> user) => Login(user.Value);
        protected virtual void Login(User user)
        {
            var menuEncounters = MenuInfoReader.GetMenuEncountersInfo(user);
            var menuSceneInfo = new LoadingMenuSceneInfo(user, LoadingScreen, menuEncounters);
            MenuDrawer.Display(menuSceneInfo);
        }


        private const string RecordNumberKey = "r";
        protected virtual void Instance_LinkActivated(LinkActivation s)
        {
            if (!s.QueryString.ContainsKey(RecordNumberKey))
                return;

            var recordNumberStr = s.QueryString[RecordNumberKey];
            if (!int.TryParse(recordNumberStr, out int recordNumber))
                return;

            SceneInfo.Result.RemoveListeners();
            EncounterQuickStarter.StartEncounter(SceneInfo.User, SceneInfo.LoadingScreen, SceneInfo.MenuEncountersInfo, recordNumber);
        }
    }
}