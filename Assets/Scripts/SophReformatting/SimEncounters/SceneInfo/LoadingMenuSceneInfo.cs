using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class LoadingMenuSceneInfo
    {
        public User User { get; }
        public ILoadingScreen LoadingScreen { get; }
        public WaitableResult<List<Category>> Categories { get; }
        public WaitableResult<MenuSceneInfo> Result = new WaitableResult<MenuSceneInfo>();

        public LoadingMenuSceneInfo(User user, ILoadingScreen loadingScreen, WaitableResult<List<Category>> categories)
        {
            User = user;
            LoadingScreen = loadingScreen;
            Categories = categories;
            Categories.AddOnCompletedListener(CategoriesRetrieved);
        }

        private void CategoriesRetrieved(List<Category> categories)
        {
            var loadedInfo = new MenuSceneInfo(this);
            Result.SetResult(loadedInfo);
        }
    }
}