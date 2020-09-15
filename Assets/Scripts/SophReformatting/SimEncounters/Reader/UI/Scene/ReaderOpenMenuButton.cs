using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderOpenMenuButton : BaseReaderSceneDrawer
    {
        public Button Button { get => button; set => button = value; }
        [SerializeField] private Button button;
        public bool ShowConfirmation { get => showConfirmation; set => showConfirmation = value; }
        [SerializeField] private bool showConfirmation = true;

        protected IUserMenuSceneStarter MenuSceneStarter { get; set; }

        [Inject]
        public virtual void Inject(IUserMenuSceneStarter menuSceneStarter) => MenuSceneStarter = menuSceneStarter;

        protected virtual void Awake() => Button.onClick.AddListener(OpenMenu);
        protected User User { get; set; }
        protected ILoadingScreen LoadingScreen { get; set; }
        public override void Display(LoadingReaderSceneInfo sceneInfo)
            => Display(sceneInfo.User, sceneInfo.LoadingScreen);
        public virtual void Display(User user, ILoadingScreen loadingScreen)
        {
            User = user;
            LoadingScreen = loadingScreen;
        }
        protected virtual void OpenMenu()
        {
            if (ShowConfirmation)
                MenuSceneStarter.ConfirmStartingMenuScene(User, LoadingScreen);
            else
                MenuSceneStarter.StartMenuScene(User, LoadingScreen);
        }
    }
}