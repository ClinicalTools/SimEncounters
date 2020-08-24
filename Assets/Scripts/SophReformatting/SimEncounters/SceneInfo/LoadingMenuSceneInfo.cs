namespace ClinicalTools.SimEncounters
{
    public class LoadingMenuSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public WaitableResult<IMenuEncountersInfo> MenuEncountersInfo { get; }
        public WaitableResult<MenuSceneInfo> Result = new WaitableResult<MenuSceneInfo>();

        public LoadingMenuSceneInfo(User user, ILoadingScreen loadingScreen, WaitableResult<IMenuEncountersInfo> menuEncountersInfo)
        {
            User = user;
            LoadingScreen = loadingScreen;
            MenuEncountersInfo = menuEncountersInfo;
            MenuEncountersInfo.AddOnCompletedListener(CategoriesRetrieved);
        }

        private void CategoriesRetrieved(WaitedResult<IMenuEncountersInfo> menuEncountersInfo)
        {
            var loadedInfo = new MenuSceneInfo(this);
            Result.SetResult(loadedInfo);
        }
    }
}