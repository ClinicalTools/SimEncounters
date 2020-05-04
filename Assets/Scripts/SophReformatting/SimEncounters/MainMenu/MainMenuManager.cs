using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuManager : SceneManager, IMenuSceneDrawer
    {
        [SerializeField] private BaseMenuSceneDrawer menuDrawer;
        public BaseMenuSceneDrawer MenuDrawer { get => menuDrawer; set => menuDrawer = value; }

        [SerializeField] private LoadingScreen loadingScreen;
        public LoadingScreen LoadingScreen { get => loadingScreen; set => loadingScreen = value; }

        protected ICategoriesReader CategoriesReader { get; set; }
        [Inject] public virtual void Inject(ICategoriesReader categoriesReader) => CategoriesReader = categoriesReader;

        protected override void StartAsInitialScene()
        {
            var user = User.Guest;
            var menuEncounters = CategoriesReader.GetCategories(user);
            var menuSceneInfo = new LoadingMenuSceneInfo(User.Guest, LoadingScreen, menuEncounters);
            MenuDrawer.Display(menuSceneInfo);
        }

        protected override void StartAsLaterScene()
        {
            Destroy(LoadingScreen.gameObject);
        }

        public void Display(LoadingMenuSceneInfo sceneInfo) => MenuDrawer.Display(sceneInfo);
    }
}