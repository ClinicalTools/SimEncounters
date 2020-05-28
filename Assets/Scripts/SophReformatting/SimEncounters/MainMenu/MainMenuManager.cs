using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuManager : SceneManager, IMenuSceneDrawer
    {
        public BaseMenuSceneDrawer MenuDrawer { get => menuDrawer; set => menuDrawer = value; }
        [SerializeField] private BaseMenuSceneDrawer menuDrawer;

        public LoadingScreen LoadingScreen { get => loadingScreen; set => loadingScreen = value; }
        [SerializeField] private LoadingScreen loadingScreen;

        protected IMenuEncountersInfoReader MenuInfoReader { get; set; }
        [Inject] public virtual void Inject(IMenuEncountersInfoReader menuInfoReader) => MenuInfoReader = menuInfoReader;

        protected override void StartAsInitialScene()
        {
            var user = User.Guest;
            var menuEncounters = MenuInfoReader.GetMenuEncountersInfo(user);
            var menuSceneInfo = new LoadingMenuSceneInfo(User.Guest, LoadingScreen, menuEncounters);
            MenuDrawer.Display(menuSceneInfo);
        }

        protected override void StartAsLaterScene() => Destroy(LoadingScreen.gameObject);

        public void Display(LoadingMenuSceneInfo sceneInfo) => MenuDrawer.Display(sceneInfo);
    }
}