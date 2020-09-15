namespace ClinicalTools.SimEncounters
{
    public interface IMenuSceneStarter
    {
        void StartScene(LoadingMenuSceneInfo loadingSceneInfo);
    }
    public interface IUserMenuSceneStarter
    {
        void StartMenuScene(User user, ILoadingScreen loadingScreen);
        void ConfirmStartingMenuScene(User user, ILoadingScreen loadingScreen);
    }
    public class UserMenuSceneStarter : IUserMenuSceneStarter
    {
        protected IMenuSceneStarter MenuSceneStarter { get; set; }
        protected IMenuEncountersInfoReader MenuInfoReader { get; set; }
        protected BaseConfirmationPopup ConfirmationPopup { get; set; }

        public UserMenuSceneStarter(
            IMenuSceneStarter menuSceneStarter,
            IMenuEncountersInfoReader menuInfoReader,
            BaseConfirmationPopup confirmationPopup)
        {
            MenuSceneStarter = menuSceneStarter;
            MenuInfoReader = menuInfoReader;
            ConfirmationPopup = confirmationPopup;
        }

        public void StartMenuScene(User user, ILoadingScreen loadingScreen)
        {
            var categories = MenuInfoReader.GetMenuEncountersInfo(user);
            var menuSceneInfo = new LoadingMenuSceneInfo(user, loadingScreen, categories);
            MenuSceneStarter.StartScene(menuSceneInfo);
        }

        protected User User { get; set; }
        protected ILoadingScreen LoadingScreen { get; set; }

        private const string ExitConfirmationTitle = "RETURN TO MAIN MENU";
        public void ConfirmStartingMenuScene(User user, ILoadingScreen loadingScreen)
        {
            User = user;
            LoadingScreen = loadingScreen;
            ConfirmationPopup.ShowConfirmation(ExitScene, ExitConfirmationTitle, "Are you sure you want to exit?");
        }
        protected virtual void ExitScene() => StartMenuScene(User, LoadingScreen);
    }
}
