using System;
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

        public LoginThing LoginHandler { get => login; set => login = value; }
        [SerializeField] private LoginThing login;

        protected IMenuEncountersInfoReader MenuInfoReader { get; set; }
        [Inject] public virtual void Inject(IMenuEncountersInfoReader menuInfoReader) => MenuInfoReader = menuInfoReader;

        protected override void Awake()
        {
            base.Awake();

            if (MenuDrawer is ILogoutHandler logoutHandler)
                logoutHandler.Logout += ShowLogin;
        }

        protected override void StartAsInitialScene()
        {
            if (LoginHandler != null)
                ShowLogin();
            else
                Login(User.Guest);
        }

        protected override void StartAsLaterScene() => Destroy(LoadingScreen.gameObject);

        public void Display(LoadingMenuSceneInfo sceneInfo) => MenuDrawer.Display(sceneInfo);

        protected virtual void Logout()
        {
            LoginHandler.Logout();
            ShowLogin();
        }
        protected virtual void ShowLogin()
        {
            var user = LoginHandler.Login(LoadingScreen);
            user.AddOnCompletedListener(Login);
        }

        protected virtual void Login(User user)
        {
            var menuEncounters = MenuInfoReader.GetMenuEncountersInfo(user);
            var menuSceneInfo = new LoadingMenuSceneInfo(user, LoadingScreen, menuEncounters);
            MenuDrawer.Display(menuSceneInfo);
        }
    }
}