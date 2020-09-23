using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderOpenMenuButton : BaseReaderSceneDrawer, IUserEncounterDrawer
    {
        public Button Button { get => button; set => button = value; }
        [SerializeField] private Button button;
        public bool ShowConfirmation { get => showConfirmation; set => showConfirmation = value; }
        [SerializeField] private bool showConfirmation = true;

        protected IUserEncounterMenuSceneStarter MenuSceneStarter { get; set; }

        [Inject]
        public virtual void Inject(IUserEncounterMenuSceneStarter menuSceneStarter) => MenuSceneStarter = menuSceneStarter;

        protected virtual void Awake() => Button.onClick.AddListener(OpenMenu);
        protected User User { get; set; }
        protected UserEncounter UserEncounter { get; set; }
        protected ILoadingScreen LoadingScreen { get; set; }
        public override void Display(LoadingReaderSceneInfo sceneInfo)
            => Display(sceneInfo.User, sceneInfo.LoadingScreen);
        public virtual void Display(User user, ILoadingScreen loadingScreen)
        {
            User = user;
            LoadingScreen = loadingScreen;
        }
        public virtual void Display(UserEncounter userEncounter) => UserEncounter = userEncounter;
        protected virtual void OpenMenu()
        {
            if (UserEncounter == null) {
                if (ShowConfirmation)
                    MenuSceneStarter.ConfirmStartingMenuScene(User, LoadingScreen);
                else
                    MenuSceneStarter.StartMenuScene(User, LoadingScreen);
            } else {
                if (ShowConfirmation)
                    MenuSceneStarter.ConfirmStartingMenuScene(UserEncounter, LoadingScreen);
                else
                    MenuSceneStarter.StartMenuScene(UserEncounter, LoadingScreen);

            }
        }
    }
}