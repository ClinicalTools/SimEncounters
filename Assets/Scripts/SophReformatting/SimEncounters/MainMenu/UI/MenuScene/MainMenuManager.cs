using ImaginationOverflow.UniversalDeepLinking;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MainMenuManager : SceneManager, IMenuSceneDrawer
    {
        public BaseMenuSceneDrawer MenuDrawer { get => menuDrawer; set => menuDrawer = value; }
        [SerializeField] private BaseMenuSceneDrawer menuDrawer;
        public LoadingScreen LoadingScreen { get => loadingScreen; set => loadingScreen = value; }
        [SerializeField] private LoadingScreen loadingScreen;
        public BaseInitialLoginHandler LoginHandler { get => login; set => login = value; }
        [SerializeField] private BaseInitialLoginHandler login;
        public GameObject WelcomeScreen { get => welcomeScreen; set => welcomeScreen = value; }
        [SerializeField] private GameObject welcomeScreen;

        protected IMenuEncountersInfoReader MenuInfoReader { get; set; }
        protected IEncounterQuickStarter EncounterQuickStarter { get; set; }
        protected QuickActionFactory LinkActionFactory { get; set; }
        [Inject]
        public virtual void Inject(IMenuEncountersInfoReader menuInfoReader, IEncounterQuickStarter encounterQuickStarter, QuickActionFactory linkActionFactory)
        {
            MenuInfoReader = menuInfoReader;
            EncounterQuickStarter = encounterQuickStarter;
            LinkActionFactory = linkActionFactory;
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

            sceneInfo.Result.AddOnCompletedListener(SceneInfoLoaded);
        }
        protected virtual void SceneInfoLoaded(WaitedResult<MenuSceneInfo> sceneInfo)
        {
            if (!sceneInfo.HasValue())
                return;
            if (sceneInfo.Value.LoadingScreen != null)
                sceneInfo.Value.LoadingScreen.Stop();
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
            Display(menuSceneInfo);
        }

        public void TestLink()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("id", "73");
            var linkActivation = new LinkActivation("lift://encounter?id=73", "id=73", dictionary);

            Instance_LinkActivated(linkActivation);
        }

        protected virtual void Instance_LinkActivated(LinkActivation s)
        {
            QuickAction quickAction = LinkActionFactory.GetLinkAction(s);
            if (quickAction.Action == QuickActionType.NA)
                return;

            SceneInfo.Result.RemoveListeners();
            EncounterQuickStarter.StartEncounter(SceneInfo.User, SceneInfo.LoadingScreen, SceneInfo.MenuEncountersInfo, quickAction.EncounterId);
        }
    }
}